// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Protobuf.Dto;
using nanoFramework.Protobuf.Utility;
using System;
using System.Collections;
using System.Diagnostics;

namespace nanoFramework.Protobuf
{
    internal static class Reader
    {
        private static readonly IBitConverter _convert = EndianBitConverter.Big;
        private static readonly Type[] _emptyTypes = new Type[0];

        public static object Deserialize(IStream stream, Type targetType)
        {
            if (!targetType.IsClass)
                throw new NotImplementedException();

            var b = stream.ReadByte();

            if (b == ProtoUtility.ObjectStart || b == ProtoUtility.InheritedObjectStart)
            {
                var timing = DateTime.UtcNow;

                var result = DeserializeObject(stream, targetType, b);

                Debug.WriteLine($"Deserialization took {DateTime.UtcNow.Subtract(timing).TotalMilliseconds} ms.");

                return result;
            }

            //unexpected byte
            throw new NotImplementedException();
        }

        private static object DeserializeObject(IStream stream, Type targetType, int b)
        {
            if(targetType == null)
            {
                //this happens if data in the stream contains a no longer mapped object
                //hence advance the stream to the end of the object
                while (true)
                {
                    b = stream.ReadByte();
                    if (b == 0x81 || b == -1)
                        return null;
                }
            }

            if (b == ProtoUtility.InheritedObjectStart)
            {
                //read next value from stream which is an integer, no need for all other parameters
                var inheritedKey = (int)ReadFromStream(stream, null, null, null);

                targetType = ProtoIncludeUtility.FindProtoInclude(targetType, inheritedKey);
            }

            var current = CreateInstance(targetType);

            var mappings = ProtoUtility.GetMappings(targetType);

            if (mappings == null)
                return current;

            return DeserializeNext(stream, targetType, current, mappings);
        }

        private static object DeserializeNext(IStream stream, Type targetType, object current, MemberMapping[] mappings)
        {
            var b = stream.ReadByte();

            if (b == 0x81 || b == -1)
                return current;

            stream.Seek(-1, System.IO.SeekOrigin.Current);

            //read next value from stream which is an integer, no need for all other parameters
            var protoMember = (int)ReadFromStream(stream, null, null, null);

            var processed = false;

            foreach (var mapping in mappings)
            {
                if (mapping.Tag == protoMember)
                {
                    var memberType = mapping.GetMemberType();
                    var val = ReadFromStream(stream, memberType, current, mappings);
                    val = Convert(val, memberType);
                    mapping.SetValue(current, val);
                    processed = true;
                    break;
                }
            }

            if(!processed)
            {
                _ = ReadFromStream(stream, null, null, null);
                Debug.WriteLine($"The data for type {targetType.Name} contains ProtoMember id {protoMember} while the type does not have this ProtoMember?");
            }

            return DeserializeNext(stream, targetType, current, mappings);
        }

        public static object Convert(object obj, Type targetType)
        {
            if (obj == null)
                return null;

            //NanoTODO #5
            //nF does not support cast from unsigned types to signed as in .Net (ushort to int for instance)
            //Fallback is string parsing...

            if (targetType == typeof(long))
            {
                if (obj is long || obj is int || obj is short || obj is byte) return (long)obj;
                return long.Parse(obj.ToString());
            }

            if (targetType == typeof(int))
            {
                if (obj is int || obj is short || obj is byte) return (int)obj;
                return int.Parse(obj.ToString());
            }

            if (targetType == typeof(short))
            {
                if(obj is short || obj is byte) return (short)obj;
                return short.Parse(obj.ToString());
            }

            return obj;
        }

        private static object ReadFromStream(IStream stream, Type targetType, object current, MemberMapping[] mappings)
        {
            var b = stream.ReadByte();

            if (b == ProtoUtility.ObjectStart || b == ProtoUtility.InheritedObjectStart)
                return DeserializeObject(stream, targetType, b);

            //positive fixint	0xxxxxxx	0x00 - 0x7f
            if (b.IsInRange(0, 0x7F)) return b;

            //map covered by 0x80 = complex type, see above

            //fixarray	1001xxxx	0x90 - 0x9f
            if (b.IsInRange(0x90, 0x9f)) return ReadArray(stream, b - 0x90, targetType, current, mappings);

            // fixstr	101xxxxx	0xa0 - 0xbf
            if (b.IsInRange(0xA0, 0xBF)) return StringUtility.ReadFromStream(stream, (byte)b);

            // negative fixnum stores 5-bit negative integer 111xxxxx
            if (b.IsInRange(0xE0, 0xFF)) return (sbyte)b;

            // null
            if (b == 0xC0) return null;

            // 0xC1 not used.

            // bool: false
            if (b == 0xC2) return false;

            // bool: true
            if (b == 0xC3) return true;

            // binary array, max 255
            if (b == 0xC4) return stream.Read((byte)stream.ReadByte());

            // binary array, max 65535
            if (b == 0xC5) return stream.Read(_convert.ToUInt16(stream.Read(2), 0));

            // binary max: 2^32-1                
            if (b == 0xC6)
                return stream.Read(_convert.ToUInt32(stream.Read(4), 0));

            // note 0xC7, 0xC8, 0xC9, not used.

            // float 32 stores a floating point number in IEEE 754 single precision floating point number     
            if (b == 0xCA) return _convert.ToSingle(stream.Read(4), 0);

            // float 64 stores a floating point number in IEEE 754 double precision floating point number        
            if (b == 0xCB) return _convert.ToDouble(stream.Read(8), 0);

            // uint8   0xcc   xxxxxxxx
            if (b == 0xCC) return (int)stream.ReadByte();

            // uint16   0xcd xxxxxxxx xxxxxxxx   
            if (b == 0xCD) return _convert.ToUInt16(stream.Read(2), 0);

            // uint32   0xce xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx  
            if (b == 0xCE) return _convert.ToUInt32(stream.Read(4), 0);

            // uint64   0xcF   xxxxxxxx (x4)
            if (b == 0xCF) return _convert.ToUInt64(stream.Read(8), 0);

            // array (uint16 length)
            if (b == 0xDC) return ReadArray(stream, _convert.ToUInt16(stream.Read(2), 0), targetType, current, mappings);

            // array (uint32 length)
            if (b == 0xDD)
                return ReadArray(stream, _convert.ToInt32(stream.Read(4), 0), targetType, current, mappings);

            // map (uint16 length)
            if (b == 0xDE) throw new NotImplementedException();

            // map (uint32 length)
            if (b == 0xDF) throw new NotImplementedException();

            //  str family
            if (b.IsInRange(0xD9, 0xDB)) return StringUtility.ReadFromStream(stream, (byte)b);

            // int8   0xD0   xxxxxxxx
            if (b == 0xD0) return (sbyte)stream.ReadByte();

            // int16   0xd1 xxxxxxxx xxxxxxxx   
            if (b == 0xD1) return _convert.ToInt16(stream.Read(2), 0);

            // int32    0xD2 xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx  
            if (b == 0xD2) return _convert.ToInt32(stream.Read(4), 0);

            // int64      0xD3 xxxxxxxx (x8)
            if (b == 0xD3) return _convert.ToInt64(stream.Read(8), 0);

            throw new NotImplementedException();
        }

        private static object CreateInstance(Type targetType)
        {
            var constructor = targetType.GetConstructor(_emptyTypes);

            if (constructor == null)
                throw new Exception($"Targettype {targetType.FullName} does not have a parameterless constructor");

            return constructor.Invoke(null);
        }

        private static object ReadArray(IStream stream, int len, Type targetType, object current, MemberMapping[] mappings)
        {
            var array = new ArrayList
            {
                Capacity = len
            };

            for (var i = 0; i < len; i++)
                array.Add(ReadFromStream(stream, targetType, current, mappings));

            return array.ToArray(targetType);
        }
    }
}
