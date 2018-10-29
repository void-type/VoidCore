# VoidCore

A core library for building domain-driven business apps on Asp.Net Core with Single Page Application support.

WARNING - this project is still in the design phase as a personal project. The API is subject to change and the version numbers may fluctuate. I will remove this warning when the project reaches a stable state.

## Features

### Domain Events

Extract logic from your controller and separate cross-cutting concerns like validation and logging. All logic for an event can be put into a single file.

Events are asynchronous. However, you can keep your synchronous domain logic clean with EventHandlerSyncAbstract.

Events, validators and post processors can be injected since events are immutable and stateless. Validators and post processors are added to an event through a decorator so different pipelines can be constructed from the same components.

```csharp
public class PersonsController : Controller
{
    // For extra credit, inject GetPerson event parts on construction and let DI handle dependencies
    public async Task<IActionResult> Get(string name)
    {
        var request = new GetPerson.Request(name);

        var result = await new GetPerson.Handler(_data, _mapper)
            .AddRequestValidator(new GetPerson.RequestValidator())
            .AddPostProcessor(new GetPerson.Logger(_logger))
            .Handle(request);

        return _responder.Respond(result);
    }
}
```

<!-- markdownlint-disable MD033 -->
<details>
    <summary>
        Show a full real-world event example
    </summary>
<!-- markdownlint-disable MD033 -->

```csharp
public class GetPerson
{
    public class Handler : EventHandlerAbstract<Request, Response>
    {
        public Handler(PersonData data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<Result<Response>> HandleInternal(Request request)
        {
            var person = await Task.Run(() => _data.Persons.Stored
                .ProjectTo<Response>(_mapper)
                .FirstOrDefault(l => l.Id == request.Id)));

            return (person != null) ?
                Result.Ok(personDto) :
                Result.Fail<Response>("Person not found.");
        }

        private readonly PersonData _data;
        private readonly IMapper _mapper;
    }

    // Immutable request
    public class Request
    {
        public Request(int id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    // Immutable response
    public class Response
    {
        public Response(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }

    // A validator for the request
    public class RequestValidator : RuleValidatorAbstract<Request>
    {
        protected override void BuildRules()
        {
            CreateRule("Id is required.", "id")
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Id));
        }
    }

    // Log it.
    public class Logger : FallibleEventLogger<Request, Response>
    {
        public Logging(ILoggingService logger) : base(logger) { }

        // Always log the incoming request
        public override void OnBoth(Request request, IFallible<Response> result)
        {
            _logger.Info($"RequestName: {request.Name}");
        }

        // Log the email of the person on success
        public override void OnSuccess(Request request, IFallible<Response> result)
        {
            _logger.Info($"Found: {result.Value.Email}");
        }

        private readonly ILoggingService _logger;
    }
}
```

Logging extends FallibleEventLogger which means that failures will be automatically logged.

There is also PostProcessorAbstract for a clean slate of all 3 channels (success, fail, and both), and IPostProcessor for a single channel (just called Process).

</details>

### Validation

A simple way to validate input models and domain requests. If you want to build your own complex validator, you can inherit from IRequestValidator that will still plug right into the DomainEvent pipeline.

RuleValidatorAbstract handles the inner logic of simple requests and allows for running the same validator against multiple entities. It is completely stateless.

```csharp
class CreatePersonValidator : RuleValidatorAbstract<Entity>
{
    protected override void BuildRules()
    {
        CreateRule("Name is required.", "name")
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        // dynamic messages
        CreateRule(p => $"Name cannot be {p.Name}.", p => nameof(p.Name).ToLower())
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        CreateRule("Phone is required for employees.", "phone")
            // ValidWhens are OR'd.
            // Any of the invalid conditions can invalidate the entity.
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Phone))
            .InvalidWhen(entity => !PhoneIsValidFormat(entity.Phone))

            // ExceptWhen suppresses any violations
            // ExceptWhens are AND'd.
            // All suppression expressions have to be true to suppress
            .ExceptWhen(entity => !entity.IsEmployee)
            .ExceptWhen(entity => entity.DoesNotHavePhone)
    }
}
```

### Results for fallible operations

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Any method that might fail can return a Result for explicit fallibility. Results can be typed or untyped to follow the CQRS principle.

```csharp
// A fallible method returns a Result<> or Result
public Result<Person> GetPersonById(int id)
{
    var person = _data.Persons.Stored.Find(id);

    if (person == null)
    {
        return Result.Fail("Person is not found.", "personIdField"));
    }

    return Result.Ok(person);
}

// Call your method and handle the result.
var result = GetPersonById(id);

if (result.IsFailed)
{
    var failures = result.Failures;
}

if (result.IsSuccess)
{
  // Generic results like Result<Person> have a Person value on success.
  var person = result.Value;
}
```

There are extension and static methods to combine results to quickly check for failures.

```csharp
IEnumerable<Result> results = CheckLotsOfThings();

var singleResult = results.Combine();

if (result.IsSuccess)
{
    return "Hooray!"
}
```

### Maybe for explicit nulls

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). The Maybe type can be used to make a null return type explicit.

Maybe also has implicit conversion from the internal type.

```csharp
// A fallible method returns a Result<> or Result
public Result<Person> GetPersonById(int id)
{
    // Implicit conversion means you don't have to change code or write wrappers.
    Maybe<Person> maybePerson = _data.Persons.Stored.FirstOrDefault(p => p.Id == id);

    if (maybePerson.HasNoValue)
    {
        return Result.Fail("Person is not found.", "personIdField"));
    }

    return Result.Ok(maybePerson.Value);
}
```

There are useful extension methods for common tasks.

```csharp
// One-line the bulk of the above method.
return maybePerson.ToResult("Person is not found.", "personIdField");

// Safely unwrap. If there is no value, this will return the default value. We want a null int? to be zero. In the end we have an integer of either 0 or value + 3.
var safelyAdded = maybeInt.Unwrap(value => value + 3, 0);

// Safe mappings. If there is no value, it will return a Maybe<Person>.None. We get a new Maybe object.
var safelyKnighted = maybePerson.Select(p => "Sir" + p.Name);

// All empty names will be replaced with Maybe<Person>.None
var filtered = maybePerson.Where(p => !string.IsNullOrEmpty(p.Name));
```

### Value Objects to alleviate primitive obsession

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Make a class that inherits from ValueObject to remove primitive obsession and give types to any logical data groups. Value Objects make it easy to compare the values of objects instead of references.

```csharp
public class Temperature : ValueObject
{
    public double Reading { get; }
    public TemperatureUnit Unit { get; }

    public Temperature(double reading, string unit)
    {
        Reading = reading;
        Unit = unit;
    }

    // It's not just a number.
    // Provide the components that make this temperature unique.
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Math.Round(Reading, 1);
        yield return Unit;
    }
}
```

### Text Search on object properties

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

### Responses for API standards

Make predictable presentation APIs with...

* User messages
* Validation failures with message and field name
* Data sets
* Data sets with pagination
* Downloadable files

VoidCore.AspNet includes HttpResponder to send a Result of the above through IActionResult to a web client.

### Configuration for Asp.Net Core

There are many helpers to build an application with...

* Serilog multi-platform file logging.
* Active Directory group authorization via Windows authentication.
* HTTPS with redirection and HSTS headers.
* Antiforgery for SPAs.
* Exception handling (SPA and MVC) with logging.
  * Api endpoints return a JSON {message: ""} object.
  * MVC will redirect to safe error or forbidden pages in non-development.
* Routing for SPA and Web API.
* Model wrappers for data repositories with default implementation for EF Core.

## Developers

You will find everything you need to build and test this project in the Scripts folder.

There is a script to install and update global tools used to develop this project.

There are also VSCode tasks for each script.

This project is not currently released, but feel free to use it as is.

To use it you can:

1. Reference it via local ProjectReference in your csproj. It will be built with your dependent project.
2. Deploy it to a local Nuget store via [nuget.exe](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)...

```powershell
cd Scripts/
./buildAllPkg.ps1
nuget init "VoidCore.Model/out" "/path/to/your/nuget/repo/"
nuget init "VoidCore.AspNet/out" "/path/to/your/nuget/repo/"
```
