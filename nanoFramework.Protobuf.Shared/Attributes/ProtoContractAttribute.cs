// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Protobuf
{
    /// <summary>
    /// The ProtoContract attribute to define an object as serializable
    /// This attribute is currently unneccesary, any object in a graph decorated with <see cref="ProtoMemberAttribute"/> will be serialized.
    /// It is still good practice to apply the attribute to stay inline with Protobuf principles and for potential future features.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ProtoContractAttribute : Attribute
    {
    }
}
