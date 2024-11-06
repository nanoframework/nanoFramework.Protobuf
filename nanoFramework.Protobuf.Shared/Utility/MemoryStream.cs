// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Protobuf.Utility
{
    /// <summary>
    /// Wrapper for MemoryStream
    /// </summary>
    internal class MemoryStream : IStream
    {
        System.IO.MemoryStream _stream;

        public MemoryStream()
        {
            _stream = new System.IO.MemoryStream();
        }

        public MemoryStream(byte[] data)
        {
            _stream = new System.IO.MemoryStream(data);
        }

        public void Dispose() => _stream?.Dispose();

        public void Write(byte[] bytes, int offset, int count) => _stream?.Write(bytes, offset, count);

        public void WriteByte(byte value) => _stream?.WriteByte(value);

        public byte[] ToArray() => _stream?.ToArray();

        public int ReadByte() => _stream == null ? -1 : _stream.ReadByte();

        public void Seek(int offset, System.IO.SeekOrigin loc) => _stream.Seek(offset, loc);

        public void Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
    }
}
