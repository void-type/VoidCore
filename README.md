# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg)](https://github.com/void-type/VoidCore/blob/main/LICENSE.txt)
[![Build Status](https://github.com/void-type/VoidCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/void-type/VoidCore/actions/workflows/ci.yml)

[VoidCore](https://github.com/void-type/voidcore) is a set of core libraries for building business applications on .NET.

[FoodStuffs](https://github.com/void-type/foodstuffs) is a comprehensive example of how applications can be built using VoidCore.

## Documentation

VoidCore is split into packages so you only get what you need.

| Docs | Description | Release |
| --- | --- | --- |
| [VoidCore.Model](docs/model.md) | An opinionated core for building business applications. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg)](https://www.nuget.org/packages/VoidCore.Model/) |
| [VoidCore.AspNet](docs/aspnet.md) | Configure ASP.NET web applications based on VoidCore.Model. Includes single-page front end support. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/) |
| [VoidCore.EntityFramework](docs/entityFramework.md) | Entity Framework Core data access for applications based on VoidCore.Model. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/) |
| [VoidCore.Finance](docs/finance.md) | A financial library with primary focus on simple amortization and time-value of money. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Finance.svg)](https://www.nuget.org/packages/VoidCore.Finance/) |

## Developers

VoidCore is built on [GitHub Actions](https://github.com/void-type/VoidCore/actions/workflows/ci.yml) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false). To release, run the CI workflow manually on main. It builds, pushes to NuGet using trusted publishing, then tags and creates the GitHub release.

To work on VoidCore, you will need the [.NET SDK](https://dotnet.microsoft.com/download) and [PowerShell](https://github.com/PowerShell/PowerShell/releases/latest).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

```powershell
./build/build.ps1
```

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
