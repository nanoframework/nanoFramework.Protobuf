using System;

namespace nanoFramework.Protobuf
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ProtoIncludeAttribute : Attribute
    {
        public ProtoIncludeAttribute(string derivedType, int key)
        {
            Key = key;
            DerivedType = derivedType;
        }

        //NanoTODO #2
        //No support for typeof() in nF yet
        //It compiles fine though but at runtime the Type property will be null

        //public ProtoIncludeAttribute(Type derivedType, int key)
        //{
        //    Key = key;
        //    DerivedType = derivedType;
        //}

        public int Key { get; }
        public string DerivedType { get; }
        //public Type DerivedType { get; }
    }
}
