namespace nanoFramework.Protobuf.Utility
{
    internal static class NumericUtility
    {
        public static void WriteToStream(IStream stream, double val)
        {
            stream.WriteByte(0xCB);
            stream.Write(ByteConverter.GetBytes(val), 0, 8);
        }

        public static void WriteToStream(IStream stream, float val)
        {
            stream.WriteByte(0xCA);
            stream.Write(ByteConverter.GetBytes(val), 0, 4);
        }

        public static void WriteToStream(IStream stream, ulong val)
        {
            stream.WriteByte(0xCF);
            byte[] dataBytes = ByteConverter.GetBytes(val);
            stream.Write(dataBytes, 0, 8);
        }

        public static void WriteToStream(IStream stream, long iVal)
        {
            if (iVal >= 0)
            {   // fixedval
                if (iVal <= 127)
                {
                    stream.WriteByte((byte)iVal);
                }
                else if (iVal <= 255)
                {  //UInt8
                    stream.WriteByte(0xCC);
                    stream.WriteByte((byte)iVal);
                }
                else if (iVal <= 0xFFFF)
                {  //UInt16
                    stream.WriteByte(0xCD);
                    stream.Write(ByteConverter.GetBytes((short)iVal), 0, 2);
                }
                else if (iVal <= 0xFFFFFFFF)
                {  //UInt32
                    stream.WriteByte(0xCE);
                    stream.Write(ByteConverter.GetBytes((int)iVal), 0, 4);
                }
                else
                {  //UInt64
                    stream.WriteByte(0xD3);
                    stream.Write(ByteConverter.GetBytes(iVal), 0, 8);
                }
            }
            else
            {
                if (iVal <= int.MinValue)  //-2147483648  // 64 bit
                {
                    stream.WriteByte(0xD3);
                    stream.Write(ByteConverter.GetBytes(iVal), 0, 8);
                }
                else if (iVal <= short.MinValue)   // -32768    // 32 bit
                {
                    stream.WriteByte(0xD2);
                    stream.Write(ByteConverter.GetBytes((int)iVal), 0, 4);
                }
                else if (iVal <= -128)   // -32768    // 16 bit
                {
                    stream.WriteByte(0xD1);
                    stream.Write(ByteConverter.GetBytes((short)iVal), 0, 2);
                }
                else if (iVal <= -32)
                {
                    stream.WriteByte(0xD0);
                    stream.WriteByte((byte)iVal);
                }
                else
                {
                    stream.WriteByte((byte)iVal);
                }
            }
        }
    }
}
