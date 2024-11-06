// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.TestFramework;

namespace nanoFramework.Protobuf.Test.Net
{
    [TestClass]
    public sealed class ProtobufTest
    {
        [TestMethod]
        public void EndToEndTest()
        {
            ProtobufTests.EndToEndTest();
        }

        [TestMethod]
        public void BigStringTest()
        {
            ProtobufTests.BigStringTest();
        }
    }
}
