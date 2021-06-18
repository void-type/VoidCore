# VoidCore.EntityFramework

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework)

## Installation

```powerShell
dotnet add package VoidCore.EntityFramework
```

## Features

Add an Entity Framework data layer to your application.

### Async Database Abstractions

Included are EntityFramework implementations of the specification and repository patterns from VoidCore.Model. These implementations defer the heavy lifting of searching, sorting, paging, and joining to the SQL Server. All calls are asynchronous.

See [VoidCore.Model](model.md) for more about it's data persistence features. You can decorate these EF repositories with Model's auditable and soft-delete decorators.

### Query Tagging

Queries are automatically tagged with the repository and specification names they were called with. This helps tie SQL logs backs to the application code. See [Query Tags on Microsoft docs](https://docs.microsoft.com/en-us/ef/core/querying/tags) for more information.
