namespace nanoFramework.Protobuf.Test.Domain
{
    [ProtoContract]
    [ProtoInclude("nanoFramework.Protobuf.Test.Domain.SomeObjectDerived, nanoFramework.Protobuf.Test", 10)]
    public class SomeObject
    {
        [ProtoMember(1)]
        public string StringProperty;

        [ProtoMember(2)]
        public int IntProperty;

        [ProtoMember(10)]
        public SomeChildObject ChildObject;

        [ProtoMember(20)]
        public SomeChildObjectDerived DerivedChildObject;
    }
}
