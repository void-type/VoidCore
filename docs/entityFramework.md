# VoidCore.EntityFramework

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework)

## Installation

```powerShell
dotnet add package VoidCore.EntityFramework
```

## Features

Add an Entity Framework data layer to your

### Async Database Abstractions

Included are EntityFramework implementations of the specification and repository patterns from VoidCore.Model. These patterns defer the heavy lifting of sorting, search, pagination, and joining to the SQL Server. All calls are asynchronous.

### Query Tagging

Queries are tagged with information about their repository and specification. Tags help match a SQL command in SQL profiler or logs to the application code. When paired with VoidCore.AspNet's logging strategy, the SQL statement is also tagged with the web request trace ID.

### SQL Logging

Apps will use the default behavior of logging EF SQL commands through the ILoggingFactory defined in your Startup.cs.
