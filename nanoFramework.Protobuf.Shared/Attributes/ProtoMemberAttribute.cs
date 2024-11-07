// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Protobuf
{
    //NanoTODO #1
    //No support for GetProperties in nF. All needed code is present so uncomment when GetProperties is available
    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]

    /// <summary>
    /// Attribute to decorate the members which need to be serialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ProtoMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes an instance of the ProtoMemberAttribute with the provided tag
        /// </summary>
        /// <param name="tag">The identifier for the decorated member. Must be unique within the type containing the field</param>
        public ProtoMemberAttribute(int tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// The identifier for the decorated member. Must be unique within the type containing the field
        /// </summary>
        public int Tag { get; set; }
    }
}
