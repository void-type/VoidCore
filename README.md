# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg?style=flat-square)](https://github.com/void-type/VoidCore/blob/master/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/VoidCore/1.svg?style=flat-square)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=master)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/VoidCore/1.svg?style=flat-square)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=master)

A set of core libraries for building domain-driven business applications.

The current major version of VoidCore includes opinionated support for Asp.Net Core 2.1, 2.2, and 3.0 applications.

[FoodStuffs](https://github.com/void-type/foodstuffs) is a comprehensive example of how applications can be built using VoidCore.

## Documentation

Read about the packages available.

| Docs | Release | Pre-Release | Description |
| --- | --- | --- | --- |
| [VoidCore.AspNet](docs/aspnet.md) | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.AspNet/) | [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.AspNet.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.AspNet) | Configure an Asp.Net Core web application with enterprise SPA conventions. |
| [VoidCore.Domain](docs/domain.md) | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Domain.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Domain/) | [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Domain.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Domain) | Domain-driven, functional and event-based development. |
| [VoidCore.EntityFramework](docs/entityFramework.md) | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.EntityFramework/) | [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework) | Implement a data layer using Entity Framework Core using the specification pattern. |
| [VoidCore.Model](docs/model.md) | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Model/) | [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model) | Services and interfaces for opinionated business applications. |
| [VoidCore.Finance](https://github.com/void-type/VoidCore.Finance/blob/master/README.md) | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Finance.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Finance/) | [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Finance.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Finance) | A financial library with primary focus on simple amortization and time-value of money. |

## Developers

VoidCore is built on [Azure Pipelines](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=master) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false).

To work on VoidCore, you will need the [.Net Core SDK](https://dotnet.microsoft.com/download).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
