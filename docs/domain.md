# VoidCore.Domain

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.Domain.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.Domain/)

## Installation

```powerShell
dotnet add package VoidCore.Domain
```

## Features

VoidCore.Domain is a basic framework for building domain-driven, event-based applications.

### Functional Extensions

Write more functional code with generic functions that help pipe objects into each other.

```csharp
// Get output from an IDisposable in a way that can be piped.
var employee = Disposable
    .Using(DbContextFactory.Create, context => context.Persons.Find("Joe"))

// Perform side-effects in your pipe while ensuring the input is passed as output.
    .Tee(p => Log(p))

// Transform one entity into another. Much like LINQ's Select for single objects rather than collections.
    .Map(p => new Employee(p.Name, p.Email));
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

There are many extension methods for making a pipeline of results.

```csharp
// Combine lots of results into a single result. Note that this returns an untyped result.
IEnumerable<Result> results = CheckLotsOfThings();

var singleResult = results.Combine();

// Transform your result into a typed one, or transform typed results to other types.
// If the original result is failed, the selector is not invoked and the failures are copied over.
var newResult = singleResult.Select(() => "Hooray!");

// Perform side-effect actions depending on result success. The original result is passed down the pipeline.
newResult
    .TeeOnSuccess(value => DoSomething(value))
    .TeeOnFailure(result => Log(result.Failures));
```

### Maybe for explicit nulls

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). The Maybe type can be used to make a null return type explicit.

Maybe also has implicit conversion from the internal type.

```csharp
// A fallible method returns a Result
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

There are useful extension methods for common Maybe tasks.

```csharp
// One-line the bulk of the above method using the static From and the ToResult extension
return Maybe.From(_data.Persons.Stored.FirstOrDefault(p => p.Id == id))
    .ToResult("Person is not found.", "personIdField");

// Safely unwrap. If there is no value, this will return the default value. We want a null int? to be zero. In the end we have an integer of either 0 or value + 3.
var safelyAdded = maybeInt.Unwrap(value => value + 3, 0);

// Safe mappings. If there is no value, it will return a Maybe<Person>.None. We get a new Maybe object.
var safelyKnighted = maybePerson.Select(p => "Sir" + p.Name);

// All persons without names will be replaced with Maybe<Person>.None
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

    // Temp is not just a number.
    // You can provide the components that make this temperature unique and comparable.
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Math.Round(Reading, 1);
        yield return Unit;
    }
}
```

### Domain Events

Extract logic from your controllers and separate cross-cutting concerns like validation and logging. All logic for an event can be put into a single file.

Events are tied to explicit request and response objects. Events are handled asynchronously by default. However, you can keep your synchronous domain logic clean of async concerns with EventHandlerSyncAbstract.

Event pipelines are created through decorator methods that add Request Validators and Post Processors. Events and their pipeline component base classes are stateless and reusable by default. If your component dependencies allow it, you can register them as singletons and reuse components in different pipelines.

```csharp
public class PersonsController : Controller
{
    // For extra credit, inject GetPerson event parts into your controller and let DI handle dependencies.
    // The components are constructed here for example clarity.
    public async Task<IActionResult> Get(string id)
    {
        var request = new GetPerson.Request(id);

        var result = await new GetPerson.Handler(_data, _mapper)
            .AddRequestValidator(new GetPerson.RequestValidator())
            .AddPostProcessor(new GetPerson.Logger(_logger))
            .Handle(request);

        // _responder is an HttpResponder from VoidCore.AspNet
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

        public override async Task<IResult<Response>> HandleInternal(Request request)
        {
            var person = await _data.Persons.Stored
                .ProjectTo<Response>(_mapper)
                .FirstOrDefaultAsync(l => l.Id == request.Id);

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

    // A rule-based validator for the request.. See below for more on this.
    public class RequestValidator : RuleValidatorAbstract<Request>
    {
        public RequestValidator()
        {
            CreateRule("Id is required.", "id")
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Id));
        }
    }

    // Log your event. See below for more on this.
    public class Logger : FallibleEventLogger<Request, Response>
    {
        public Logging(ILoggingService logger) : base(logger) { }

        // Always log the incoming request
        public override void OnBoth(Request request, IResult<Response> result)
        {
            _logger.Info($"RequestName: {request.Name}");
        }

        // Log the email of the person on success
        public override void OnSuccess(Request request, IResult<Response> result)
        {
            _logger.Info($"Found: {result.Value.Email}");
        }

        private readonly ILoggingService _logger;
    }
}
```

</details>

### Simple Rule Validation

A simple way to validate input models and domain requests. If you want to build your own complex validator, you can inherit from IRequestValidator.

RuleValidatorAbstract handles the inner logic of simple requests and allows for running the same validator against multiple entities. It is completely stateless.

```csharp
class CreatePersonValidator : RuleValidatorAbstract<Entity>
{
    public CreatePersonValidator()
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

### Event Post Processors

Post Processors fire after the handling of an event or after validation failure. Post Processors should not change the response of the event; however, they can fire off commands such as notifications and logging.

* Inherit from PostProcessorAbstract to define different processing for three channels of the Result<Response>: Success, Fail, Both.
* Inherit from IPostProcessor for a single channel that fires on both types of Result.

There are default logging Post Processor implementations in the VoidCore.Model library. See above for an example.
