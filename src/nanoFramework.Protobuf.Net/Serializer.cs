namespace nanoFramework.Protobuf
{
    public partial class Serializer
    {
        public T Deserialize<T>(byte[] serialized) => (T)Deserialize(typeof(T), serialized);
    }
}
