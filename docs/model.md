# VoidCore.Model

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Model/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model)

## Installation

```powerShell
dotnet add package VoidCore.Model
```

## Features

VoidCore.Model is an opinionated set of services and interfaces to support business application domains.

### API Responses

Make predictable data APIs.

* User messages
* Failures with user message and optional field name
* Data sets
* Paginated data sets
* Downloadable files

### Data Persistence

* Asynchronous repositories with read/write control and specification-based queries.
* Soft delete on entities.
* Auditable entities via Created and Modified names/dates.

### Emailing

* Service interface for sending emails from the domain layer.
* Template emails using the builder pattern. Templates can be implemented with html or text emails.

### Logging

* Domain-safe, platform-agnostic interfaces for logging services.
* PostProcessors for all out-of-the-box Response types that include fallible logging.
* Inherit your custom PostProcessors from FallibleEventLogger to automatically log Result failures from the validation or event handler.

### Common Service Interfaces

Interfaces for common services that the domain can use.

* Time
* Current user
* Emailing
