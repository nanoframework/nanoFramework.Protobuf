// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Reflection;
using nanoFramework.Protobuf.Dto;

namespace nanoFramework.Protobuf.Utility
{
    internal static class PropertyUtility
    {
        public static void Map(Type targetType, ArrayList mappings)
        {
            var properties = GetProperties(targetType);

            if (properties == null || properties.Length == 0)
                return;

            for (int i = 0; i < properties.Length; i++)
            {
                var protoMember = properties[i].GetProtoMember();

                if (protoMember == null) continue;

                mappings.Add(new MemberMapping(properties[i])
                {
                    ProtoInclude = ProtoIncludeUtility.FindProtoInclude(properties[i].DeclaringType == null ? targetType : properties[i].DeclaringType),
                    ProtoMember = protoMember.Tag,
                });
            }
        }

        private static PropertyInfo[] GetProperties(Type type)
        {
            var list = new ArrayList();

            GetProperties(type, list);

            return (PropertyInfo[])list.ToArray(typeof(PropertyInfo));
        }

        private static void GetProperties(Type type, ArrayList list)
        {
            if (type == null) return;

            //NanoTODO #1
            //var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var properties = new PropertyInfo[0];

            foreach (var property in properties)
                if (property.DeclaringType == type) //skip properties not declared on type, we will find it in a later recursion
                    list.Add(property);

            GetProperties(type.BaseType, list);
        }
    }
}
