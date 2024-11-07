// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Protobuf.Test.Domain;

namespace nanoFramework.Protobuf.Test
{
    public static class ObjectProvider
    {
        public static SomeObject CreateObject()
        {
            return new SomeObjectDerived
            {
                StringProperty = "abc",
                IntProperty = 123,
                DerivedProperty = true,
                ChildObject = new SomeChildObject
                {
                    StringProperty = "def",
                    IntProperty = 456
                },
                DerivedChildObject = new SomeChildObjectDerived
                {
                    StringProperty = "ghi",
                    IntProperty = 789,
                    DerivedProperty = true
                }
            };
        }

    }
}
