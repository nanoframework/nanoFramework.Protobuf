// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Protobuf.Test.Domain
{
    [ProtoContract]
    public class SomeChildObjectDerived : SomeChildObject
    {
        [ProtoMember(3)]
        internal bool DerivedProperty;
    }
}
