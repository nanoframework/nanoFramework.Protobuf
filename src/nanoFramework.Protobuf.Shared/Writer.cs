using nanoFramework.Protobuf.Dto;
using nanoFramework.Protobuf.Utility;
using System;
using System.Diagnostics;

namespace nanoFramework.Protobuf
{
    internal static class Writer
    {
        public static void Serialize(object obj, IStream stream)
        {
            var timing = DateTime.UtcNow;

            Serialize(stream, obj, null);

            Debug.WriteLine($"Serialization took {DateTime.UtcNow.Subtract(timing).TotalMilliseconds} ms.");
        }

        private static void Serialize(IStream stream, object obj, Type memberType)
        {
            var type = obj.GetType();

            if (memberType != null && memberType.IsEnum)
                throw new NotImplementedException();
            else
            {
                var mappings = ProtoUtility.GetMappings(type);

                if (mappings == null || mappings.Length == 0)
                    EncodeToStream(obj, memberType, type, stream);
                else
                    ConvertComplexType(obj, memberType, stream, mappings);
            }
        }

        private static void ConvertComplexType(object obj, Type memberType, IStream stream, MemberMapping[] mappings)
        {
            var objType = obj.GetType();

            var protoInclude = ProtoIncludeUtility.FindProtoInclude(objType);

            if (protoInclude < 0)
            {
                if (memberType != null && memberType != objType)
                    throw new Exception($"No ProtoInclude attribute found on the base types of {objType.FullName} which is set on a member of type {memberType.FullName}");

                stream.WriteByte(ProtoUtility.ObjectStart);
            }
            else
            {
                stream.WriteByte(ProtoUtility.InheritedObjectStart);

                NumericUtility.WriteToStream(stream, protoInclude);
            }

            foreach (var mapping in mappings)
            {
                var value = mapping.GetValue(obj);

                if (value == null) continue;

                NumericUtility.WriteToStream(stream, mapping.Tag);

                Serialize(stream, value, mapping.GetMemberType());
            }

            stream.WriteByte(ProtoUtility.ObjectEnd);
        }

        private static void EncodeToStream(object obj, Type memberType, Type type, IStream stream)
        {
            if(obj == null)
            {
                stream.WriteByte(0xC0);
                return;
            }    

            if(type.IsArray)
            {
                WriteArray(stream, (object[])obj, memberType);
                return;
            }

            if(obj is byte[] bytes)
            {
                WriteBinary(stream, bytes);
                return;
            }

            if (type.IsEnum)
            {
                //NanoTODO #4
                //Enums not supported as nanoframework FieldInfo.SetValue does not allow setting integers to an enum and no Enum.ToObject exists yet
                //Note: while serializing here is not an issue, there's no way to set the value to the Enum when deserializing
                //WriteInteger(stream, (int)obj);
                //return;
                throw new NotSupportedException($"Type {type.Name} is an Enum which is not supported yet.");
            }

            switch (type.FullName)
            {
                case "System.String":
                    var str = obj.ToString();
                    if(str != null)
                        StringUtility.WriteToStream(stream, str);
                    break;
                case "System.Int16":
                    NumericUtility.WriteToStream(stream, (short)obj);
                    break;
                case "System.UInt16":
                    NumericUtility.WriteToStream(stream, (ushort)obj);
                    break;
                case "System.Int32":
                    NumericUtility.WriteToStream(stream, (int)obj);
                    break;
                case "System.UInt32":
                    NumericUtility.WriteToStream(stream, (uint)obj);
                    break;
                case "System.Int64":
                    NumericUtility.WriteToStream(stream, (long)obj);
                    break;
                case "System.UInt64":
                    NumericUtility.WriteToStream(stream, (ulong)obj);
                    break;
                case "System.Boolean":
                    WriteBoolean(stream, (bool)obj);
                    break;
                case "System.Single":
                    NumericUtility.WriteToStream(stream, (float)obj);
                    break;
                case "System.Double":
                    NumericUtility.WriteToStream(stream, (double)obj);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void WriteArray(IStream stream, Array array, Type memberType)
        {
            byte b;
            byte[] lenBytes;
            int len = array.Length;
            if (len <= 15)
            {
                b = (byte)(0x90 + (byte)len);
                stream.WriteByte(b);
            }
            else if (len <= 65535)
            {
                b = 0xDC;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes((ushort)len);
                stream.Write(lenBytes, 0, lenBytes.Length);
            }
            else
            {
                b = 0xDD;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes(len);
                stream.Write(lenBytes, 0, lenBytes.Length);
            }

            foreach (var item in array)
                Serialize(stream, item, memberType);
        }

        private static void WriteBinary(IStream stream, byte[] rawBytes)
        {
            int len = rawBytes.Length;
            byte[] lenBytes;
            byte b;
            if (len <= 255)
            {
                b = 0xC4;
                stream.WriteByte(b);
                b = (byte)len;
                stream.WriteByte(b);
            }
            else if (len <= 65535)
            {
                b = 0xC5;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes(Convert.ToUInt16(len.ToString()));
                stream.Write(lenBytes, 0, lenBytes.Length);
            }
            else
            {
                b = 0xC6;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes(Convert.ToUInt32(len.ToString()));
                stream.Write(lenBytes, 0, lenBytes.Length);
            }
            stream.Write(rawBytes, 0, rawBytes.Length);
        }

        private static void WriteBoolean(IStream stream, bool val)
        {
            stream.WriteByte((byte)(val ? 0xC3 : 0xC2));
        }
    }
}
