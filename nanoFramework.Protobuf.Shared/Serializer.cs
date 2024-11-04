// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Protobuf.Utility;
using System;

namespace nanoFramework.Protobuf
{
    /// <summary>
    /// The Serializer class providing serialization and deserialization capabilities
    /// </summary>
    public partial class Serializer : IAmASerializer
    {
        StreamType _streamType = StreamType.MemoryStream;

        /// <summary>
        /// Initializes an instance of Serializer.
        /// This defaults to using a MemoryStream internally
        /// </summary>
        public Serializer() { }

        /// <summary>
        /// Initializes an instance of Serializer
        /// </summary>
        /// <param name="streamType">The stream type to use. MemoryStream is the preferred option, ProtobufStream is there to support payloads > 65kB.</param>
        public Serializer(StreamType streamType) => _streamType = streamType;

        /// <summary>
        /// Deserializes a byte array to the given type
        /// </summary>
        /// <param name="type">The type to deserialize to</param>
        /// <param name="serialized">The byte array containing the serialized payload</param>
        /// <returns>An object graph of Type type</returns>
        public object Deserialize(Type type, byte[] serialized)
        {
            using (var stream = CreateStream(serialized))
                return Reader.Deserialize(stream, type);
        }

        /// <summary>
        /// Serializes an object graph to a byte array
        /// </summary>
        /// <param name="obj">The object graph to serialize</param>
        /// <returns>A byte array containing the serialized payload</returns>
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

    /// <summary>
    /// The StreamType enum providing the possible IStream implementation options
    /// </summary>
    public enum StreamType
    {
        MemoryStream,
        ProtobufStream
    }
}
