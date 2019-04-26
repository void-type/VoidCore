# VoidCore.Model

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Model/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget&style=flat-square)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model)

## Installation

```powerShell
dotnet add package VoidCore.Model
```

## Features

VoidCore.Model is an opinionated set of services and interfaces to support business application domains.

### Text Search on Object Properties

Check many properties of an object for an array of terms. Just supply the properties you want to include.

SearchTerms will split the search string on whitespace by default. Or you can specify a string separator. Also, you can supply your own array of terms.

SearchCriteria.PropertiesContain returns an expression that you can pass into an ORM like Entity Framework for deferred evaluation.

```csharp
IQueryable<Person> people = GetPeople();
var searchString = "find all these words in any single property";

var matchedPeople = people
    .Where(SearchCriteria.PropertiesContain<TestObject>(
        new SearchTerms(searchString),
        obj => obj.FirstName,
        obj => obj.LastName,
        obj => obj.Biography
    ));
```

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

### Logging

* Domain-safe, platform-agnostic interfaces for logging services.
* PostProcessors for all out-of-the-box Response types that include fallible logging.
* Inherit your custom PostProcessors from FallibleEventLogger to automatically log Result failures from the validation or event handler.

### Common Service Interfaces

Interfaces for common services that the domain can use.

* Time
* Current user
* Emailing
