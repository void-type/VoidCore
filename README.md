# VoidCore

A set of core libraries for building domain-driven business applications. Includes opinionated support for Asp.Net Core applications.

WARNING - this project is still in the design phase as a personal project. The API is subject to change and the version numbers may fluctuate. I will remove this warning when the project reaches a stable state.

## Installation

Use nuget.org to get the latest version. Install each package as needed.

```powerShell
dotnet add package VoidCore.AspNet
dotnet add package VoidCore.Domain
dotnet add package VoidCore.Model
```

## Documentation

Read about the packages available.

[VoidCore.AspNet](docs/aspnet.md) - configure an Asp.Net Core web application.

[VoidCore.Domain](docs/domain.md) - domain-driven and event-based development.

[VoidCore.Model](docs/model.md) - services and interfaces for opinionated business applications.

## Developers

To begin, you will need to install some global tools. To do this easily, just run the following:

```powershell
cd build/
./installAndUpdateTools.ps1
```

See the /build folder for scripts used to develop, test and build this project.

There are VSCode tasks for each script. The VSCode build task will build all solution nuget packages into the /artifacts folder.
