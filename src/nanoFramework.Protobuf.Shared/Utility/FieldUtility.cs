// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Protobuf.Dto;
using System;
using System.Collections;
using System.Reflection;

namespace nanoFramework.Protobuf.Utility
{
    internal static class FieldUtility
    {
        public static void Map(Type targetType, ArrayList mappings)
        {
            var fields = GetFields(targetType);

            if (fields == null || fields.Length == 0)
                return;

            for (int i = 0; i < fields.Length; i++)
            {
                var protoMember = fields[i].GetProtoMember();

                if (protoMember == null) continue;

                mappings.Add(new MemberMapping(fields[i])
                {
                    ProtoInclude = ProtoIncludeUtility.FindProtoInclude(fields[i].DeclaringType == null ? targetType : fields[i].DeclaringType),
                    ProtoMember = protoMember.Tag,
                });
            }
        }

        private static FieldInfo[] GetFields(Type type)
        {
            var list = new ArrayList();

            GetFields(type, list);

            return (FieldInfo[])list.ToArray(typeof(FieldInfo));
        }

        private static void GetFields(Type type, ArrayList list)
        {
            if (type == null) return;

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
                if (field.DeclaringType == type) //skip fields not declared on type, we will find it in a later recursion
                    list.Add(field);

            GetFields(type.BaseType, list);
        }
    }
}
