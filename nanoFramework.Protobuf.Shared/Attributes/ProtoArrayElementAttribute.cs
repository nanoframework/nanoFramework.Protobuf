// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Protobuf
{
    /// <summary>
    /// Use this attribute on Array fields to make the element type known
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ProtoArrayElementAttribute : Attribute
    {
        /// <summary>
        /// ProtoArrayElementAttribute constructor
        /// </summary>
        /// <param name="arrayElementType">The fully qualified type name of the Array element type</param>
        public ProtoArrayElementAttribute(string arrayElementType)
        {
            ArrayElementType = arrayElementType;
        }

        /// <summary>
        /// The fully qualified type name of the Array element type
        /// </summary>
        public string ArrayElementType { get; }
    }
}
