# nanoFramework.Protobuf

This library is usefull for projects looking to transfer data from or to embedded systems which are using [.NET nanoFramework](https://github.com/nanoframework/Home). It provides assemblies compiled with nanoFramework and netstandard 2.0 which makes it usable in both nanoFramework projects and about any other .Net flavor.

While transferring json is a viable option, the goal of this library is to provide high performance serialization and deserialization with the smallest possible payload. The benefits are obvious when regarding the limited capacity of embedded systems regarding RAM and storage.

The library mimicks the principles of [Protobuf-net](https://github.com/protobuf-net/protobuf-net). There are a lot of bells and whistles in Protobuf-net which are not needed (so far) or do not make sense in the context of nanoFramework. Hence some features are lacking or shortcuts have been taken. A noteworthy choice in this regard is the format of the binary data which is stored in accordance with the [MessagePack](https://msgpack.org/) spec instead of the less concise Protobuf-net format.

Similarly a number of current limitations of the nanoFramework API means there are some restrictions to the current implementation:

 1. Only fields can be decorated with ProtoMember. NanoFramework is lacking Type.GetProperties at the moment. All code to support properties is present in the codebase so this can be easily added when GetProperties is implemented.
 2. The ProtoInclude attribute offers an opportunity to describe inherited types on the base type so that they are properly serialized. The typeof() syntax does compile in nanoFramework but yields an empty Type at runtime. Hence the derived types must be specified as a string containing the fully qualified type.
 3. The current implementation of Type.GetElementType returns null at runtime which prevents from detecting the array element type. An additional attribute ProtoArrayElementAttribute is provided to get around this.
 4. There is no Enum.ToObject implementation yet in nanoFramework which means Enums are not supported currently. This can be solved by casting an Enum to an integer in a wrapper class.
 5. Conversions from unsigned types to signed types with the same or less byte size is not supported in nanoFramework (ushort to int for instance). While this has no consequences on a functional level, it does on the performance side since it is currently solved with string parsing.
 6. Lastly, the nF MemoryStream is limited to 65kB currently. Since the payload may be bigger an interface IStream is implemented in either a wrapper around MemoryStream (the preferred approach) or, when in need of (de)serializing a bigger payload, an alternative ProtobufStream implementation is available. See also the BigString test. The Serializer class defaults to the MemoryStream wrapper.

With these limitations in mind. The library requires specific contracts which means existing dto's can probably not be reused as such. This is easily circumvented by creating wrapper classes though.

The first five limitations mentioned are denoted in the code with a comment (look for //NanoTODO #).
