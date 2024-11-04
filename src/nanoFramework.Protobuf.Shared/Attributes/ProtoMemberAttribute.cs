using System;

namespace nanoFramework.Protobuf
{
    //NanoTODO #1
    //No support for GetProperties in nF. All needed code is present so uncomment when GetProperties is available
    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ProtoMemberAttribute : Attribute
    {
        public ProtoMemberAttribute(int tag)
        {
            Tag = tag;
        }

        public int Tag { get; set; }

        public override string ToString()
        {
            return Tag.ToString();
        }
    }
}
