using System;

namespace nanoFramework.Protobuf
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ProtoArrayElementAttribute : Attribute
    {
        public ProtoArrayElementAttribute(string arrayElementType)
        {
            ArrayElementType = arrayElementType;
        }

        public string ArrayElementType { get; }
    }
}
