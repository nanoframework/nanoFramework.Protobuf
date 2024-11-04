// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

namespace nanoFramework.Protobuf.Utility
{
    internal static class StringUtility
    {
        private const int ChunkSize = 65000;
        private static readonly Encoding _encoding = Encoding.UTF8;

        //fixstr stores a byte array whose length is upto 31 bytes:
        //|101XXXXX|  data  |
        //
        //str 8 stores a byte array whose length is upto (2^8)-1 bytes:
        //|  0xd9  |YYYYYYYY|  data  |
        //
        //str 16 stores a byte array whose length is upto (2^16)-1 bytes:
        //|  0xda  |ZZZZZZZZ|ZZZZZZZZ|  data  |
        //
        //str 32 stores a byte array whose length is upto (2^32)-1 bytes:
        //|  0xdb  |AAAAAAAA|AAAAAAAA|AAAAAAAA|AAAAAAAA|  data  |
        //
        //where
        //* XXXXX is a 5-bit unsigned integer which represents N
        //* YYYYYYYY is a 8-bit unsigned integer which represents N
        //* ZZZZZZZZ_ZZZZZZZZ is a 16-bit big-endian unsigned integer which represents N
        //* AAAAAAAA_AAAAAAAA_AAAAAAAA_AAAAAAAA is a 32-bit big-endian unsigned integer which represents N
        //* N is the length of data

        public static void WriteToStream(IStream stream, string value)
        {
            byte[] rawBytes = _encoding.GetBytes(value);
            int len = rawBytes.Length;
            byte b;
            byte[] lenBytes;
            if (len <= 31)
            {
                b = (byte)(0xA0 + (byte)len);
                stream.WriteByte(b);
            }
            else if (len <= 255)
            {
                b = 0xD9;
                stream.WriteByte(b);
                b = (byte)len;
                stream.WriteByte(b);
            }
            else if (len <= 65535)
            {
                b = 0xDA;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes(Convert.ToUInt16(len.ToString()));
                stream.Write(lenBytes, 0, lenBytes.Length);
            }
            else
            {
                b = 0xDB;
                stream.WriteByte(b);
                lenBytes = ByteConverter.GetBytes(Convert.ToUInt32(len.ToString()));
                stream.Write(lenBytes, 0, lenBytes.Length);
            }

            stream.Write(rawBytes, 0, rawBytes.Length);
        }

        public static string ReadFromStream(IStream stream, byte b)
        {
            int len = 0;
            if ((b >= 0xA0) && (b <= 0xBF))
                len = (byte)(b - 0xA0);
            else if (b == 0xD9)
                len = (byte)stream.ReadByte();
            else if (b == 0xDA)
                len = ByteConverter.GetUshort(stream.Read(2));
            else if (b == 0xDB)
                return stream.ReadString(ByteConverter.GetUint(stream.Read(4)));

            return stream.ReadString(len);
        }
    }
}
