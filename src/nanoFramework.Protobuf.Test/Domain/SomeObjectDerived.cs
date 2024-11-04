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
