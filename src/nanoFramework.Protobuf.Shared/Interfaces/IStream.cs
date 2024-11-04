using System;
using System.IO;

namespace nanoFramework.Protobuf.Utility
{
    internal interface IStream : IDisposable
    {
        void Read(byte[] buffer, int offset, int count);
        int ReadByte();
        void Seek(int offset, SeekOrigin loc);
        byte[] ToArray();
        void Write(byte[] bytes, int offset, int count);
        void WriteByte(byte value);
    }
}