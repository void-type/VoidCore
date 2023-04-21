# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg)](https://github.com/void-type/VoidCore/blob/main/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/VoidCore/1/main)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/VoidCore/1/main)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main)

[VoidCore](https://github.com/void-type/voidcore) is a set of core libraries for building business applications on .NET 7.

[FoodStuffs](https://github.com/void-type/foodstuffs) is a comprehensive example of how applications can be built using VoidCore.

## Documentation

VoidCore is split into packages so you only get what you need.

| Docs | Description | Release |
| --- | --- | --- |
| [VoidCore.Model](docs/model.md) | An opinionated core for building business applications. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg)](https://www.nuget.org/packages/VoidCore.Model/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model) |
| [VoidCore.AspNet](docs/aspnet.md) | Configure ASP.NET web applications based on VoidCore.Model. Includes single-page front end support. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.AspNet.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.AspNet) |
| [VoidCore.EntityFramework](docs/entityFramework.md) | Entity Framework Core data access for applications based on VoidCore.Model. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework) |
| [VoidCore.Finance](docs/finance.md) | A financial library with primary focus on simple amortization and time-value of money. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Finance.svg)](https://www.nuget.org/packages/VoidCore.Finance/) [![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Finance.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Finance) |

## Developers

VoidCore is built on [Azure Pipelines](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=1&branchName=main) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false).

To work on VoidCore, you will need the [.NET SDK](https://dotnet.microsoft.com/download) and [PowerShell](https://github.com/PowerShell/PowerShell/releases/latest).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

```powershell
./build/build.ps1
```

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
