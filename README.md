# VoidCore

A set of core libraries for building domain-driven business applications. Includes opinionated support for Asp.Net Core applications.

WARNING - this project is still in the design phase as a personal project. The API is subject to change and the version numbers may fluctuate. I will remove this warning when the project reaches a stable state.

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

These packages aren't yet released on a public feed. You can consume the packages either:

1. Referencing a project csproj via local ProjectReference in your project's csproj. The project will be built with your dependent project.
2. Deploying built packages it to a local Nuget feed via [nuget.exe](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe):

```powershell
cd build/
./buildAllPkg.ps1
nuget init "../artifacts" "/path/to/your/nuget/repo/"
```
