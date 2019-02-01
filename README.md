# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg?style=flat-square)](https://github.com/void-type/VoidCore/blob/master/LICENSE.txt)
[![Build Status](https://img.shields.io/azure-devops/build/void-type/VoidCore/3.svg?style=flat-square)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=3&branchName=master)
[![Test Coverage](https://img.shields.io/azure-devops/coverage/void-type/VoidCore/3.svg?style=flat-square)](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=3&branchName=master)

A set of core libraries for building domain-driven business applications. Includes opinionated support for Asp.Net Core applications.

| Warning |
| --- |
| This project is still in the design phase as a personal project. The API is subject to change and the version numbers may fluctuate. I will remove this warning when the project reaches a stable state. |

## Documentation

Read about the packages available.

[VoidCore.AspNet](docs/aspnet.md) - configure an Asp.Net Core web application.

[VoidCore.Domain](docs/domain.md) - domain-driven and event-based development.

[VoidCore.Model](docs/model.md) - services and interfaces for opinionated business applications.

## Developers

VoidCore is built on [Azure Pipelines](https://dev.azure.com/void-type/VoidCore/_build/latest?definitionId=3&branchName=master) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false).

To work on VoidCore, you will need the [.Net Core SDK 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1) installed.

You will also need some global tools. To install them easily, just run the following:

```powershell
cd build/
./installAndUpdateTools.ps1
```

See the /build folder for scripts used to test and build this project.

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
