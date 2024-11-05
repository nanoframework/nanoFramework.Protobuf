[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_lib-nanoframework.WebServer&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_lib-nanoframework.WebServer) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_lib-nanoframework.WebServer&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_lib-nanoframework.WebServer) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.WebServer.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.WebServer/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** Protobuf repository

## Build status[text](README.md)

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Protobuf | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Protobuf/_apis/build/status/nanoFramework.Protobuf?repoName=nanoframework%2FnanoFramework.Protobuf&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Protobuf/_build/latest?definitionId=65&repoName=nanoframework%2FnanoFramework.Protobuf&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Protobuf.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Protobuf/) |

## nanoFramework.Protobuf

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

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** WebServer library is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
