using nanoFramework.TestFramework;
using System.Diagnostics;

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
