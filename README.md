# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg)](https://github.com/void-type/VoidCore/blob/main/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/VoidCore/1.svg)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/VoidCore/1.svg)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main)

A set of core libraries for building domain-driven business applications on .NET 5 and .NET Core 3.1.

[FoodStuffs](https://github.com/void-type/foodstuffs) is a comprehensive example of how applications can be built using VoidCore.

## Documentation

Read about the packages available.

| Docs | Description | Release |
| --- | --- | --- |
| [VoidCore.AspNet](docs/aspnet.md) | Configure ASP.NET Core web applications based on VoidCore.Model. Includes single-page front end support. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.AspNet.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.AspNet) |
| [VoidCore.Domain](docs/domain.md) | Core for building domain-driven, functional and event-based applications. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Domain.svg)](https://www.nuget.org/packages/VoidCore.Domain/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Domain.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Domain) |
| [VoidCore.EntityFramework](docs/entityFramework.md) | Entity Framework Core data access for applications based on VoidCore.Model. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework) |
| [VoidCore.Finance](docs/finance.md) | A financial library with primary focus on simple amortization and time-value of money. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Finance.svg)](https://www.nuget.org/packages/VoidCore.Finance/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Finance.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Finance) |
| [VoidCore.Model](docs/model.md) | Service interfaces for building opinionated business applications. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg)](https://www.nuget.org/packages/VoidCore.Model/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model) |

## Developers

VoidCore is built on [Azure Pipelines](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false).

To work on VoidCore, you will need the [.NET SDK](https://dotnet.microsoft.com/download).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

```powershell
cd build
./build.ps1
```

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
