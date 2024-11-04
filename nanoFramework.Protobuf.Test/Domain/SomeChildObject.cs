namespace nanoFramework.Protobuf.Test.Domain
{
    [ProtoContract]
    [ProtoInclude("nanoFramework.Protobuf.Test.Domain.SomeChildObjectDerived, nanoFramework.Protobuf.Test", 10)]
    public class SomeChildObject
    {
        [ProtoMember(1)]
        public string StringProperty;

        [ProtoMember(2)]
        public int IntProperty;
    }
}
