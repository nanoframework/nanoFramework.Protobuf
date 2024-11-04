// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace nanoFramework.Protobuf.Utility
{
    /// <summary>
    /// An interface providing the necessary stream capabilities for Protobuf
    /// </summary>
    internal interface IStream : IDisposable
    {
        /// <summary>
        /// Read a number of bytes into a buffer at the given offset
        /// </summary>
        /// <param name="buffer">The buffer to fill</param>
        /// <param name="offset">The offset to start reading</param>
        /// <param name="count">The number of bytes to read</param>
        void Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads a single byte from the stream
        /// </summary>
        /// <returns>The read byte as an integer</returns>
        int ReadByte();

        /// <summary>
        /// Moves the position in the stream with the given offset considering the given origin
        /// </summary>
        /// <param name="offset">The number of positions to move</param>
        /// <param name="loc">The origin to start the seek from</param>
        void Seek(int offset, SeekOrigin loc);

        /// <summary>
        /// Converts the contents of the stream to a byte array
        /// </summary>
        /// <returns>A byte array containing the contents of the stream</returns>
        byte[] ToArray();

        /// <summary>
        /// Writes a number of bytes to the stream
        /// </summary>
        /// <param name="bytes">The byte array containing the bytes to write</param>
        /// <param name="offset">The position in the byte array to start at</param>
        /// <param name="count">The number of bytes to write</param>
        void Write(byte[] bytes, int offset, int count);

        /// <summary>
        /// Writes a single byte to the stream
        /// </summary>
        /// <param name="value">The byte to write</param>
        void WriteByte(byte value);
    }
}