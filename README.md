# VoidCore

A core library for building domain-driven business apps on Asp.Net Core with Single Page Application support.

## Domain Events

Extract logic from your controller and separate cross-cutting concerns like logging and validation. All logic for an event can be put into a single file.

Events, validators and post processors can be injected since events are immutable and stateless. Validators and post processors are added through a decorator.

```csharp
public class PersonsController : Controller
{
    // For extra credit, inject GetPerson event parts on construction
    public IActionResult Get(string name)
    {
        var request = new GetPerson.Request(name);

        var result = new GetPerson.Handler(_data, _mapper)
            .AddRequestValidator(new GetPerson.RequestValidator())
            .AddPostProcessor(new GetPerson.Logging(_logger))
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
    public class Handler : DomainEventAbstract<Request, Response>
    {
        public Handler(PersonData data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        protected override Result<Response> HandleInternal(Request request)
        {
            var person = _data.Persons
                .ProjectTo<Response>(_mapper)
                .FirstOrDefault(l => l.Id == request.Id));

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
    public class RequestValidator : ValidatorAbstract<Request>
    {
        protected override void BuildRules()
        {
            CreateRule("Id is required.", "id")
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Id));
        }
    }

    // Log it.
    public class Logging : FallibleLogging<Request, Response>
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

        // Logging : FallibleLogging which means that failures will be automatically logged.
        // There is also PostProcessorAbstract for a clean slate of all 3 channels, and IPostProcessor for a single channel (Process).

        private readonly ILoggingService _logger;
    }
}
```

</details>

## Validation

```csharp
class CreatePersonValidator : ValidatorAbstract<Entity>
{
    protected override void BuildRules()
    {
        CreateRule("Name is required.", "name")
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        CreateRule("Phone is required for employees.", "phone")
            // ValidWhens are OR'd.
            // Any of the invalid conditions can invalidate the entity.
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Phone))
            .InvalidWhen(entity => !PhoneIsValidFormat(entity.Phone))

            // ExceptWhen supresses any violations
            // ExceptWhens are AND'd.
            // All suppression expressions have to be true to suppress
            .ExceptWhen(entity => !entity.IsEmployee)
            .ExceptWhen(entity => entity.DoesNotHavePhone)
    }
}
```

## Results for Fallible Operations

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Any method that might fail can return a Result for explicit fallibility. Results can be typed or untyped to follow the CQRS principle.

```csharp
// A fallible method
public Result<Person> GetPersonById(int id)
{
    var person = _data.Persons.Find(id);

    if (person == null)
    {
        Result.Fail("Person is not found.", "personIdField"));
    }

    Result.Ok(person);
}

// Call your method and handle the result.
var result = GetPersonById(id);

if (result.IsFailed)
{
    var failures = result.Failures;
}

if (result.IsSuccess)
{
  var person = result.Value;
}

// Combine multiple results to check for failures
IEnumerable<Result> results = CheckLotsOfThings();

var singleResult = results.Combine();

if (result.IsSuccess)
{
    return "Hooray!"
}

```

## Text Search on Object Properties

Check many properties of an object for text. It will split any string on whitespace, or it can take an explicit array of terms.

```csharp
IQueryable<Entity> entities = GetEntities();
var searchTerms = "find all words in any single property";

var matchedEntities = entities
    .SearchStringProperties(
        searchTerms,
        obj => obj.StringProperty1,
        obj => obj.StringProperty2,
        obj => obj.StringProperty3
    );
```

## Standardized Responses

Make predictable APIs with...

* Useer messages
* Validation failures with message and field name
* Data sets
* Pagination
* Downloadable files

Includes a responder for IActionResult Web API that can be ported to any API pipeline.

## Asp.Net Core Configuration

There are many helpers to build an application with...

* Serilog multiplatform file logging.
* Active Directory group authorization.
* HTTPS with redirection and HSTS headers.
* Antiforgery for SPAs.
* Exception handling (SPA and MVC) with logging.
  * Api endpoints return a JSON {message: ""} object.
  * MVC will redirect to safe error or forbidden pages in non-development.
* Routing for SPA and Web API.
* Data layer wrapper with default support for EF Core.

## Developers

You will find everything you need to build and test this project in the Scripts folder.

There are also VSCode tasks for each script.

This project is not currently released, but feel free to use it as is. You can reference it via local ProjectReference or deploy it to a local Nuget store via

```powershell
cd Scripts/
./buildAllPkg.ps1
nuget init "out" "/path/to/your/nuget/repo/"
```
