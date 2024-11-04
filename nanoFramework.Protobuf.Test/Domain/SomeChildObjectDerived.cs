namespace nanoFramework.Protobuf.Test.Domain
{
    [ProtoContract]
    public class SomeChildObjectDerived : SomeChildObject
    {
        [ProtoMember(3)]
        internal bool DerivedProperty;
    }
}
