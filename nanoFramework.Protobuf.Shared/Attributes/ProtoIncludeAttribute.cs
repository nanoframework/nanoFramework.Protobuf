// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Protobuf
{
    /// <summary>
    /// Attribute to indicate derived types that are to be serialized. This attribute needs to go on the base type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ProtoIncludeAttribute : Attribute
    {
        /// <summary>
        /// Initializes an instance of ProtoIncludeAttribute
        /// </summary>
        /// <param name="derivedType">The fully qualified type name of the derived type</param>
        /// <param name="key">The unique identifier within the scope of the base type for all fields of the derived type</param>
        public ProtoIncludeAttribute(string derivedType, int key)
        {
            Key = key;
            DerivedType = derivedType;
        }

        //NanoTODO #2
        //No support for typeof() in nF yet
        //It compiles fine though but at runtime the Type property will be null

        //public ProtoIncludeAttribute(Type derivedType, int key)
        //{
        //    Key = key;
        //    DerivedType = derivedType;
        //}

        /// <summary>
        /// The unique identifier within the scope of the base type for all fields of the derived type
        /// </summary>
        public int Key { get; }

        /// <summary>
        /// The fully qualified type name of the derived type
        /// </summary>
        public string DerivedType { get; }
        //public Type DerivedType { get; }
    }
}
