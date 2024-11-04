using System;

namespace nanoFramework.Protobuf
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ProtoContractAttribute : Attribute
    {
    }
}
