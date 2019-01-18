# VoidCore.Model

## Install

```powerShell
dotnet add package VoidCore.Model
```

## Features

VoidCore.Model is an opinionated set of services and interfaces to support business application domains.

### Text Search on Object Properties

Check many properties of an object for an array of terms. It will split any search string on whitespace, or it can take an explicit array of terms.

```csharp
IQueryable<Person> people = GetPeople();
var searchString = "find all these words in any single property";

var matchedPeople = people
    .SearchStringProperties(
        searchString,
        obj => obj.FirstName,
        obj => obj.LastName,
        obj => obj.Biography
    );
```

### API Responses

Make predictable data APIs.

* User messages
* Failures with user message and optional field name
* Data sets
* Paginated data sets
* Downloadable files

### Data Persistence

* Repositories with read/write control.
* Soft delete.
* Auditable entities via Created and Modified names/dates.

### Logging

* Inherit your custom PostProcessors from FallibleEventLogger to automatically log Result failures from the validation or event.
* There are also PostProcessors for all API Response types that include fallible logging.
* Domain-safe, platform-agnostic interfaces for logging services.

### Common Service Interfaces

Interfaces for common services that the domain can use.

* Time
* Current user
* Emailing
