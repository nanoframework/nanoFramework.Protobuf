// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Protobuf
{
    public partial class Serializer
    {
        /// <summary>
        /// Deserializes a byte array to the given type
        /// </summary>
        /// <typeparam name="T">The type to deserialize to</param>
        /// <param name="serialized">The byte array containing the serialized payload</param>
        /// <returns>An object graph of Type T</returns>
        public T Deserialize<T>(byte[] serialized) => (T)Deserialize(typeof(T), serialized);
    }
}
