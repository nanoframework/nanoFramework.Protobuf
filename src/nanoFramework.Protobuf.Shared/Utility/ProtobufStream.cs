using System;
using System.Collections;

namespace nanoFramework.Protobuf.Utility
{
    /// <summary>
    /// Substitute for MemoryStream since nF MemoryStream is limited to 65535 bytes
    /// </summary>
    internal class ProtobufStream : IStream
    {
        Hashtable _data = null;
        byte[] _buffer = null;
        int _length = 0;
        int _position = 0;

        public ProtobufStream() 
        {
            _data = new Hashtable();
        }

        public ProtobufStream(byte[] data) 
        {
            _buffer = data;
        }

        public void Dispose() => _data?.Clear();

        public void Write(byte[] bytes, int offset, int count)
        {
            _data.Add(_data.Count, bytes);
            _length += bytes.Length;
        }

        public void WriteByte(byte value)
        {
            _data.Add(_data.Count, value);
            _length++;
        }

        public byte[] ToArray()
        {
            var result = new byte[_length];
            var pos = 0;

            for (int i = 0; i < _data.Count; i++)
            {
                var data = _data[i];

                if (data is byte)
                    result[pos++] = (byte)data;
                else if (data is byte[])
                {
                    var array = (byte[])data;
                    Array.Copy(array, 0, result, pos, array.Length);
                    pos += array.Length;
                }
                _data[i] = null;
            }

            return result;
        }

        public int ReadByte() => _position == _buffer.Length ? -1 : _buffer[_position++];

        public void Seek(int offset, System.IO.SeekOrigin loc) => _position += offset;

        public void Read(byte[] buffer, int offset, int count)
        {
            _position += offset;
            Array.Copy(_buffer, _position, buffer, 0, count);
            _position += count;
        }
    }
}
