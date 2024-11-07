// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace nanoFramework.Protobuf.Utility
{
    internal static class ProtoIncludeUtility
    {
        private static readonly Hashtable ProtoIncludeKeyDictionary = new Hashtable();
        private static readonly Hashtable ProtoIncludeTypeDictionary = new Hashtable();

        public static Type FindProtoInclude(Type type, int inheritedKey)
        {
            var key = $"{type.FullName}{inheritedKey}";

            var cached = ProtoIncludeKeyDictionary[key];
            if (cached is Type cachedType) return cachedType;

            var result = FindProtoIncludeInternal(type, inheritedKey);

            ProtoIncludeKeyDictionary.Add(key, result);

            System.Diagnostics.Debug.WriteLine($"Registering type {(result == null ? "<unknown>" : result.FullName)} with key {inheritedKey}");

            return result;
        }

        public static int FindProtoInclude(Type typeToFind)
        {
            if (typeToFind == null) return -1;

            var cached = ProtoIncludeTypeDictionary[typeToFind];
            if (cached is int key) return key;

            var result = FindProtoIncludeInternal(typeToFind.BaseType, typeToFind);

            //check for duplicates
            if (result > 0)
            {
                foreach (var item in ProtoIncludeTypeDictionary.Keys)
                {
                    var type = item as Type;
                    if (type?.BaseType == typeToFind.BaseType && ProtoIncludeTypeDictionary[type] is int existing && existing == result)
                        throw new Exception($"{typeToFind.FullName} and {type.FullName} both have ProtoInclude {existing}... You ought to know better!");
                }

                System.Diagnostics.Debug.WriteLine($"Registering key {result} for type {(typeToFind == null ? "<unknown>" : typeToFind.FullName)}");
            }

            ProtoIncludeTypeDictionary.Add(typeToFind, result);

            return result;
        }

        private static Type FindProtoIncludeInternal(Type type, int inheritedKey)
        {
            if (type == null) return null;

            var attributes = type.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is ProtoIncludeAttribute protoAttribute)
                {
                    var derivedType = Type.GetType(protoAttribute.DerivedType);
                    if (protoAttribute.Key == inheritedKey)
                        return derivedType;

                    var result = FindProtoIncludeInternal(derivedType, inheritedKey);
                    if (result != null)
                        return result;
                }
            }

            return FindProtoIncludeInternal(type.BaseType, inheritedKey);
        }

        private static int FindProtoIncludeInternal(Type typeToInvestigate, Type typeToFind)
        {
            if (typeToInvestigate == null) return -1;

            var attributes = typeToInvestigate.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                if (attribute is ProtoIncludeAttribute protoAttribute)
                {
                    if (protoAttribute.Key == 0)
                        throw new Exception($"A ProtoInclude of 0 on {typeToFind.FullName} is not supported.");

                    var derivedType = Type.GetType(protoAttribute.DerivedType);
                    if (derivedType == typeToFind)
                        return protoAttribute.Key;
                }
            }

            return FindProtoIncludeInternal(typeToInvestigate.BaseType, typeToFind);
        }
    }
}
