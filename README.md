[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Protobuf&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Protobuf) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoFramework.Protobuf&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoFramework.Protobuf) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.Protobuf.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Protobuf/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** Protobuf repository

This repository contains the Protobuf library for the .NET **nanoFramework**. It provides high-performance serialization and deserialization with the smallest possible payload, making it ideal for embedded systems with limited resources.

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.Protobuf | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.Protobuf/_apis/build/status/nanoFramework.Protobuf?repoName=nanoframework%2FnanoFramework.Protobuf&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.Protobuf/_build/latest?definitionId=117&repoName=nanoframework%2FnanoFramework.Protobuf&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Protobuf.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Protobuf/) |

## .NET nanoFramework.Protobuf

This library is useful for projects looking to transfer data to or from embedded systems using the [.NET nanoFramework](https://github.com/nanoframework/Home). It provides assemblies compiled with .NET **nanoFramework** and netstandard 2.0, making it usable in both .NET **nanoFramework** projects and other .NET flavors.

While JSON is a viable option for data transfer, the goal of this library is to provide high-performance serialization and deserialization with the smallest possible payload. This is beneficial given the limited RAM and storage capacity of embedded systems.

The library mimics the principles of [Protobuf-net](https://github.com/protobuf-net/protobuf-net). Many features of protobuf-net are not needed (so far) or do not make sense in the context of .NET **nanoFramework**. Hence, some features are lacking or shortcuts have been taken. A noteworthy choice in this regard is the format of the binary data, which is stored in accordance with the [MessagePack](https://msgpack.org/) spec instead of the less concise .NET **nanoFramework** format.

Due to current limitations of the .NET **nanoFramework** API, there are some restrictions in the current implementation:

1. Only fields can be decorated with `ProtoMember`. .NET **nanoFramework** lacks `Type.GetProperties` at the moment. All code to support properties is present in the codebase, so this can be easily added when `GetProperties` is implemented.
2. The `ProtoInclude` attribute allows describing inherited types on the base type so that they are properly serialized. The `typeof()` syntax does compile in .NET **nanoFramework** but yields an empty `Type` at runtime.
3. The current implementation of `Type.GetElementType` returns `null` at runtime which prevents from detecting the array element type. An additional attribute `ProtoArrayElementAttribute` is provided to get around this.
4. There is no `Enum.ToObject` implementation in .NET **nanoFramework** which means enums are not currently supported. This can be solved by casting an enum to an integer in a wrapper class.
5. Conversions from unsigned types to signed types with the same or less byte size are not supported in .NET **nanoFramework** (`ushort` to `int` for instance). While this has no consequences on a functional level, it does on the performance side since it is currently solved with string parsing.
6. Lastly, the .NET **nanoFramework** `MemoryStream` is limited to 65kB. Since the payload may be bigger an interface `IStream` is implemented in either a wrapper around `MemoryStream` (the preferred approach) or, when in need of (de)serializing a bigger payload, an alternative `ProtobufStream` implementation is available. See also the `BigString` unit test. The `Serializer` class defaults to the `MemoryStream` wrapper.

With these limitations in mind, the library requires specific contracts, which means existing DTOs can probably not be reused as such. This is easily circumvented by creating wrapper classes, though.

The first five limitations mentioned are denoted in the code with a comment (look for `//NanoTODO`).

## Acknowledgements

The initial version of the protobuf library was coded by [Klaus Vancamelbeke](https://github.com/KlausVcb), who has kindly handed over the library to the .NET **nanoFramework** project.

## Feedback and documentation

For documentation, providing feedback, issues, and finding out how to contribute, please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** WebServer library is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
