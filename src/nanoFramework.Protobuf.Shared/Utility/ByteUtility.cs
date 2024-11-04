using System;
using System.Text;

namespace nanoFramework.Protobuf.Utility
{
    internal static class ByteUtility
    {
        public static bool IsInRange(this int b, int start, int end)
        {
            return b >= start && b <= end;
        }
    }
}
