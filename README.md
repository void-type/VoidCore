# VoidCore
Core library for building business apps on Asp.Net with support for SPA applications.

## Currently in very experimental stages and should not be used in production.

## Validation
```csharp
class EntityValidator : ValidatorAbstract<Entity>
{
    protected override void BuildRules()
    {
        CreateRule("Name is required.", "name")
            .ValidWhen(entity => !string.IsNullOrWhitespace(entity.Name));

        CreateRule("Phone is required for employees.", "phone")
            .ValidWhen(entity => !string.IsNullOrWhitespace(entity.Phone))
            // ValidWhens and ExceptWhens are AND'd together.
            .ValidWhen(entity => PhoneIsValidFormat(entity.Phone))
            // ExceptWhen supresses any violations
            .ExceptWhen(entity => !entity.IsEmployee)
    }
}
```

## Results for fallible operations
Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions)
```csharp
Result<User> GetUserById(int id)
{
    var user = _data.Users.Find(id);

    if (user == null)
    {
        Result.Fail(new Failure("User is not found."));
    }

    Result.Ok(user);
}

var result = GetUserById(id);

if (result.IsFailed)
{
    var failures = result.Failures;
}

if (result.IsSuccess)
{
  var user = result.Value;
}
```

## Search object properties for text
```csharp
IQueryable<Entity> entities = GetEntities();
var searchTerms = "find all words in any single property";

var matchedEntities = entities.SearchStringProperties(
                searchTerms,
                obj => obj.StringProperty1,
                obj => obj.StringProperty2,
                obj => obj.StringProperty3
            );
```

## Standard responses
Downloadable files, user messages, data arrays and pagination.

## Standard Asp.Net Configuration
There are many helpers to build an application with...
 * Serilog multiplatform file logging
 * Active Directory groups
 * HTTPS with redirection and HSTS headers
 * Antiforgery with SPA
 * Exception handling (SPA and MVC)
   * Api endpoints return a JSON {Message: ""} object.
   * MVC will redirect to safe error or forbidden pages in non-development.
 * Routing for SPA and web api
