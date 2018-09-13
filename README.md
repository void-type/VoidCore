# VoidCore

A core library for building domain-driven business apps on Asp.Net Core with Single Page Application support.

## Domain Events

Extract logic from your controller and separate cross-cutting concerns like logging and validation. All logic for an event can be put into a single file.

Events, validators and post processors can be injected since events are immutable and stateless. Validators and post processors are added through a decorator.

```csharp
public class LoaneesController : Controller
{
    // Inject domain event parts into ctor and set as private readonly fields.

    public IActionResult GetWithDependencyInjection(string name)
    {
        var request = new GetLoanee.Request(name);

        var result = _getLoaneeHandler
            .AddRequestValidator(_getLoaneeRequestValidator)
            .AddPostProcessor(_getLoaneeLogging)
            .Handle(request);

        return _responder.Respond(result);
    }
}
```

<!-- markdownlint-disable MD033 -->
<details>
    <summary>
        Show Event Code
    </summary>
<!-- markdownlint-disable MD033 -->

```csharp
public class LoaneesController : Controller
{
    public IActionResult Get(string name)
    {
        var request = new GetLoanee.Request(name);

        var result = new GetLoanee.Handler(_data, _mapper)
            .AddRequestValidator(new GetLoanee.RequestValidator())
            .AddPostProcessor(new GetLoanee.Logging(_logger))
            .Handle(request);

        return _responder.Respond(result);
    }
}


public class GetLoanee
{
    public class Handler : DomainEventAbstract<Request, Response>
    {
        public Handler(LoaneeData data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        protected override async Task<Result<Response>> HandleInternal(Request request)
        {
            var loanee = await Task.FromResult(_data.Loanees
                .Where(l => l.Name == request.Name)
                .FirstOrDefault());

            if (request.Name == "throw")
            {
                throw new Exception("blow up");
            }

            if (loanee == null)
            {
                return Result.Fail<Response>("Loanee not found.");
            }

            var loaneeDto = _mapper.Map<Loanee, Response>(loanee);

            return Result.Ok(loaneeDto);
        }

        private readonly LoaneeData _data;
        private readonly IMapper _mapper;
    }

    // Immutable request
    public class Request
    {
        public Request(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    // Immutable response using ctor param conventions
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
            CreateRule("Name is required.", "name")
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Name));

            CreateRule("Double trouble about name.", "name")
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Name));
        }
    }

    // Log it.
    public class Logging : FallibleLoggingPostProcessor<Request, Response>
    {
        public Logging(ILoggingService logger) : base(logger) { }

        public override void OnBoth(Request request, IFallible<Response> result)
        {
            _logger.Info($"RequestName: {request.Name}");
        }

        public override void OnSuccess(Request request, IFallible<Response> result)
        {
            _logger.Info($"Found: {result.Value.Email}");
        }

        private readonly ILoggingService _logger;
    }
}
```

</details>

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
            // ValidWhens are OR'd.
            // Any of the invalid conditions can invalidate the entity.
            .InvalidWhen(entity => !PhoneIsValidFormat(entity.Phone))
            // ExceptWhen supresses any violations
            .ExceptWhen(entity => !entity.IsEmployee)
            // ExceptWhens are AND'd.
            // All suppression expressions have to be true to suppress
            .ExceptWhen(entity => entity.DoesNotHavePhone)
    }
}
```

## Results for Fallible Operations

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Any method that might fail can return a Result for explicit fallibility. Results can be typed or untyped to follow the CQRS principle.

```csharp
// A fallible method
public Result<User> GetUserById(int id)
{
    var user = _data.Users.Find(id);

    if (user == null)
    {
        Result.Fail("User is not found.", "userField"));
    }

    Result.Ok(user);
}

// Call your method and handle the result.
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

## Text Search on Object Properties

In the future this should work on the database with EF Core.

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

* User messages
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
