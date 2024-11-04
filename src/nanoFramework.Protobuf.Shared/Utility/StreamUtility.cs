using System;
using System.Text;

namespace nanoFramework.Protobuf.Utility
{
    internal static class StreamUtility
    {
        private const int ChunkSize = 65535;

        public static byte[] Read(this IStream stream, int len)
        {
            byte[] buffer = new byte[len];
            stream.Read(buffer, 0, len);
            return buffer;
        }

        public static byte[] Read(this IStream stream, uint len)
        {
            byte[] buffer = new byte[len];

            var position = 0;

            while (len > 0)
            {
                var bytesToRead = len > ChunkSize ? ChunkSize : (int)len;
                var chunk = stream.Read(bytesToRead);
                Array.Copy(chunk, 0, buffer, position, bytesToRead);
                position += bytesToRead;
                len -= (uint)bytesToRead;
            }

            return buffer;
        }

        public static string ReadString(this IStream stream, int len)
        {
            return Encoding.UTF8.GetString(stream.Read(len), 0, len);
        }

        public static string ReadString(this IStream stream, uint len)
        {
            var str = string.Empty;

            var position = 0;

            while (len > 0)
            {
                var bytesToRead = len > ChunkSize ? ChunkSize : (int)len;
                str += Encoding.UTF8.GetString(stream.Read(bytesToRead), 0, bytesToRead);
                position += bytesToRead;
                len -= (uint)bytesToRead;
            }

            return str;
        }
    }
}
