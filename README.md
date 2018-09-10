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
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        CreateRule("Phone is required for employees.", "phone")
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Phone))
            // ValidWhens are OR'd. Any of the invalid conditions can invalidate the entity.
            .InvalidWhen(entity => !PhoneIsValidFormat(entity.Phone))
            // ExceptWhen supresses any violations
            .ExceptWhen(entity => !entity.IsEmployee)
            // ExceptWhens are AND'd. All suppression expressions have to be true to suppress
            .ExceptWhen(entity => entity.DoesNotHavePhone)
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
        Result.Fail("User is not found.", "userField"));
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

## Framework for simplifying domain events to pull logic out of the controller.
See the VoidCore branch of my [MediatrRailwayExample](https://github.com/void-type/MediatrRailwayExample/tree/VoidCore) project. Particularly the EndpointDepsController and GetLoanee files.

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
Downloadable files, user messages, data arrays and pagination that make the API predictable for SPA clients.

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
