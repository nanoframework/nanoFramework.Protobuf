// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Reflection;

namespace nanoFramework.Protobuf.Dto
{
    internal class MemberMapping
    {
        private static readonly Hashtable ArrayElementDictionary = new Hashtable();

        public MemberMapping(FieldInfo field) => Field = field;

        public MemberMapping(PropertyInfo property) => Property = property;

        private FieldInfo Field { get; set; }
        private PropertyInfo Property { get; set; }

        public int ProtoMember { get; set; }
        public int ProtoInclude { get; internal set; }

        public int Tag => ProtoInclude > 0 ? ProtoInclude + ProtoMember : ProtoMember;

        public string Name => Field?.Name ?? Property?.Name;
        public Type DeclaringType => Field?.DeclaringType ?? Property?.DeclaringType;

        public Type GetMemberType()
        {
            if (Field != null && Field.FieldType.IsArray) return GetArrayElement(Field);

            if (Property != null && Property.PropertyType.IsArray) return GetArrayElement(Property);

            return Field?.FieldType ?? Property?.PropertyType;
        }

        public object GetValue(object obj)
        {
            return Field?.GetValue(obj) ?? Property?.GetValue(obj, null);
        }

        public override string ToString()
        {
            return $"{Field?.Name}{Property?.Name} - {Tag}";
        }

        private Type GetArrayElement(MemberInfo member)
        {
            //NanoTODO #3
            //no support in nanoframework for GetElementType (it returns null)
            //replace the below code and remove ProtoArrayElementAttribute when implemented
            //return Field?.FieldType.GetElementType() ?? Property?.PropertyType.GetElementType();

            var cached = (Type)ArrayElementDictionary[member];
            if (cached != null) return cached;

            //first attempt: if the field.Name ends with s,
            //remove the s and look for the remainder as a type in the assembly of the DeclaringType
            //works in a lot of cases ;-)
            if (member.Name.EndsWith("s"))
            {
                var typeName = member.Name.Substring(0, 1).ToUpper() + member.Name.Substring(1, member.Name.Length - 2);

                var types = member.DeclaringType.Assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.Name == typeName)
                    {
                        ArrayElementDictionary.Add(member, type);
                        return type;
                    }
                }
            }

            var attributes = member.GetCustomAttributes(true);

            ProtoArrayElementAttribute protoArrayElement = null;

            foreach (var attribute in attributes)
                if (attribute is ProtoArrayElementAttribute protoAttribute)
                    protoArrayElement = protoAttribute;

            if (protoArrayElement == null)
                throw new Exception($"No support for GetElementType in nanoFramework means you must specify the array element type for field {member.Name}");

            var result = Type.GetType(protoArrayElement.ArrayElementType);

            ArrayElementDictionary.Add(member, result);

            return result;
        }

        internal void SetValue(object current, object val)
        {
            Field?.SetValue(current, val);
            Property?.SetValue(current, val, null);
        }
    }
}
