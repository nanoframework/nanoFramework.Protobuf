using System;

namespace nanoFramework.Protobuf
{
    public interface IAmASerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(Type type, byte[] serialized);
    }
}
