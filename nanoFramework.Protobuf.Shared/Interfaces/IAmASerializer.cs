// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Protobuf
{
    /// <summary>
    /// Interface denoting serialization and deserialization capabilities.
    /// Intended to keep implementations for different platforms aligned
    /// </summary>
    public interface IAmASerializer
    {
        /// <summary>
        /// Serializes an object graph to a byte array
        /// </summary>
        /// <param name="obj">The object graph to serialize</param>
        /// <returns>A byte array containing the serialized payload</returns>
        byte[] Serialize(object obj);

        /// <summary>
        /// Deserializes a byte array to the given type
        /// </summary>
        /// <param name="type">The type to deserialize to</param>
        /// <param name="serialized">The byte array containing the serialized payload</param>
        /// <returns>An object graph of Type type</returns>
        object Deserialize(Type type, byte[] serialized);
    }
}
