using nanoFramework.Protobuf.Utility;
using System;

namespace nanoFramework.Protobuf
{
    public partial class Serializer : IAmASerializer
    {
        StreamType _streamType = StreamType.MemoryStream;

        public Serializer() { }

        public Serializer(StreamType streamType)
        {
            _streamType = streamType;
        }

        public object Deserialize(Type type, byte[] serialized)
        {
            using (var stream = CreateStream(serialized))
                return Reader.Deserialize(stream, type);
        }

        public byte[] Serialize(object obj)
        {
            using (var stream = CreateStream())
            {
                Writer.Serialize(obj, stream);

                return stream.ToArray();
            }
        }

        private IStream CreateStream(byte[] data = null)
        {
            switch (_streamType)
            {
                case StreamType.MemoryStream:
                    return data == null ? new MemoryStream() : new MemoryStream(data);
                case StreamType.ProtobufStream:
                    return data == null ? new ProtobufStream() : new ProtobufStream(data);
            }

            throw new NotImplementedException();
        }
    }

    public enum StreamType
    {
        MemoryStream,
        ProtobufStream
    }
}
