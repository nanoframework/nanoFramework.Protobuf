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
