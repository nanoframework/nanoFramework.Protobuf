// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Protobuf.Utility
{
    internal static class ByteConverter
    {
        private static readonly IBitConverter _convert = EndianBitConverter.Big;

        public static byte[] GetBytes(ushort value) => _convert.GetBytes(value);
        public static byte[] GetBytes(short value) => _convert.GetBytes(value);
        public static byte[] GetBytes(uint value) => _convert.GetBytes(value);
        public static byte[] GetBytes(int value) => _convert.GetBytes(value);
        public static byte[] GetBytes(ulong value) => _convert.GetBytes(value);
        public static byte[] GetBytes(long value) => _convert.GetBytes(value);
        public static byte[] GetBytes(double value) => _convert.GetBytes(value);
        public static byte[] GetBytes(float value) => _convert.GetBytes(value);

        public static ushort GetUshort(byte[] bytes) => _convert.ToUInt16(bytes, 0);
        public static uint GetUint(byte[] bytes) => _convert.ToUInt32(bytes, 0);
    }
}
