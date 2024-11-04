// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
