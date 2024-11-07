// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Protobuf.Test.Domain
{
    [ProtoContract]
    [ProtoInclude("nanoFramework.Protobuf.Test.Domain.SomeObject, nanoFramework.Protobuf.Test", 10)]
    public class SomeObjectDerived : SomeObject
    {
        [ProtoMember(3)]
        public bool DerivedProperty;
    }
}
