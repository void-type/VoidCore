# VoidCore.Domain

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.Domain.svg)](https://www.nuget.org/packages/VoidCore.Domain/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Domain.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Domain)

## Installation

```powerShell
dotnet add package VoidCore.Domain
```

## Features

VoidCore.Domain is a basic core library for building domain-driven, event-based applications.

### Functional Extensions

Write more functional code with generic functions that help pipe objects into each other.

```csharp
// Build functional pipelines from any object to reduce noisy intermediate variables.
var employee = database.GetPerson("Joe")

    // Perform side-effects in your pipe while ensuring the input is passed as output.
    .Tee(p => Log(p))

    // Transform one entity into another. Much like LINQ's Select for single objects rather than collections.
    .Map(p => new Employee(p.Name, p.Email));

// There are variants for building async pipelines.
// If the function passed to the method returns an Async Task, it will be automatically awaited.
var employee = database.GetPersonAsync("Joe")
    .TeeAsync(p => Log(p))
    .TeeAsync(p => AuthorizeAsync(p))
    .MapAsync(p => new Employee(p.Name, p.Email));
```

### Results for fallible operations

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Any method that might fail can return a Result for explicit fallibility. Results can be typed or untyped depending if a return value is needed.

```csharp
// A fallible method returns a IResult<> or IResult
public IResult<Person> GetPersonById(int id)
{
    var person = _data.Persons.GetById(id);

    if (person is null)
    {
        return Result.Fail(new Failure("Person is not found.", "personIdField"));
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
  // Generic results like IResult<Person> have a Person value on success.
  var person = result.Value;
}
```

There are many extension methods for making a pipeline of results.

```csharp
// Combine lots of results into a single result. Note that this always returns an untyped result.
IEnumerable<IResult> results = CheckLotsOfThings();
IResult singleResult = results.Combine();

// There are async versions of all extensions.
// Note, asynchronous tasks of IResult can be run in parallel if not awaited.
IResult result = await Result.CombineAsync(await DoSomeFallibleTaskAsync(), await CheckSomethingAsync());

var newResult = singleResult
    // Transform your result into a typed one, or transform typed results to other types.
    // If the original result is failed, the selector is not invoked and the failures are copied over.
    .Select(() => "Everything went well.");

    // Perform side-effect actions depending on result success. The original result is passed down the pipeline.
    .TeeOnSuccess(value => DoSomethingOnSuccess(value))
    .TeeOnFailure(result => Log(result.Failures))

    // You can also do something on both using the generic functional extensions.
    .Tee(result => DoSomethingNoMatterWhat(result))

    // Then is call "bind" in other programming languages. We can call another result-returning function without wrapping it a nested result.
    .Then(result => CheckSomethingElse(result);
```

### Maybe for explicit nulls

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). The Maybe type can be used to make a null return type explicit.

Maybe also has implicit conversion from the internal type.

```csharp
public IResult<Person> GetPersonById(int id)
{
    // When an external call can return a null, we can capture it in a Maybe.
    // Implicit conversion means you don't have to change code or write wrappers.
    Maybe<Person> maybePerson = _data.Persons.GetById(id);

    if (maybePerson.HasNoValue)
    {
        return Result.Fail(new PersonNotFoundFailure());
    }

    return Result.Ok(maybePerson.Value);
}
```

There are useful extension methods to make pipelines of Maybes and even convert them to Results. There are also async variants of all extensions.

```csharp
// Create a maybe from anything.
var maybePerson = Maybe.From(_data.Persons.GetById(id))

    // Filter on a predicate. A maybe that doesn't match will be replaced with Maybe<Person>.None.
    .Where(p => p.Name == "Patrick Stewart")

    // Then will bind your maybe into another maybe-returning function.
    // This allows you to use the output of one maybe function as input for another and prevents nested maybes.
    .Then(p => _data.Actors.GetFromPerson(p))

    // Safe mappings. If there is no value, it will return a Maybe<Person>.None.
    .Select(p => p.Name = "Sir " + p.Name);

// Safely extract the inner value. If there is no value, unwrap will return the default value of that type, or the optionally specified value.
Person captain = maybePerson.Unwrap(new Person {Name = "James Kirk" }); // We will either have Sir Patrick Stewart or James Kirk at this point.

// Alternatively, we can return a Result based on the value of the Maybe.
IResult result = maybePerson.ToResult(new PersonNotFoundFailure());
```

### Value Objects to alleviate primitive obsession

Adapted from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions). Make a class that inherits from ValueObject to remove primitive obsession and give types to any logical data groups. Value Objects make it easy to compare the values of object properties instead of references.

```csharp
// Compare many properties and ensure equality across all of them.
public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string ZipCode { get; }

    public Address(string street, string city, string state, string country, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}

// Check a single computed property that is comparable across different units.
public class Temperature : ValueObject
{
    public enum UnitType
    {
        F,
        C,
        K
    }

    public double Reading { get; }
    public UnitType Unit { get; }
    public double Kelvin
    {
        get
        {
            switch (Unit)
            {
                case UnitType.C:
                    return Math.Round(Reading + 273.15, 2);
                case UnitType.F:
                    return Math.Round((Reading - 32.0) * 5 / 9 + 273.15, 2);
                default:
                    return Math.Round(Reading, 2);
            }
        }
    }

    public Temperature(double reading, UnitType unit)
    {
        Reading = reading;
        Unit = unit;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Kelvin;
    }
}
```

### Domain Events

Extract logic from your controllers and separate cross-cutting concerns like validation and logging. All logic for an event can be put into a single file.

Events are tied to explicit request and response objects. Events are handled asynchronously by default. However, you can keep your synchronous domain logic clean of async concerns with EventHandlerSyncAbstract.

Event pipelines are created through decorator methods that add Request Validators and Post Processors. Events and their pipeline component base classes are stateless and reusable by default. If your component dependencies allow it, you can register them as singletons and reuse components in different pipelines.

Below is an example of using an event within an Asp.Net MVC controller.

```csharp
public class PersonsController : Controller
{
    public PersonsController(GetPerson.Handler getHandler, GetPerson.RequestValidator getValidator, GetPerson.Logger getLogger)
    {
        _getHandler = getHandler;
        _getValidator = getValidator;
        _getLogger = getLogger;
    }

    public async Task<IActionResult> Get(string id)
    {
        // For POST requests, the request can be extracted directly [FromBody]
        var request = new GetPerson.Request(id);

        return await _getHandler
            .AddRequestValidator(_getValidator)
            .AddPostProcessor(_getLogger)
            .Handle(request)

            // HttpResponder will handle the conversion of IResult<PersonDto> to a 200/500 IActionResult
            .MapAsync(HttpResponder.Respond);
    }
}
```

<!-- markdownlint-disable MD033 -->
<details>
    <summary>
        Show a full event example
    </summary>
<!-- markdownlint-disable MD033 -->

```csharp
public class GetPerson
{
    public class Handler : EventHandlerAbstract<Request, PersonDto>
    {
        public Handler(CompanyData data)
        {
            _data = data;
        }

        public override async Task<IResult<PersonDto>> Handle(Request request)
        {
            var personById = new PersonSpecification(p => p.Id == request.Id);

            // Persons.Get returns a Maybe<Person> from the IReadOnlyRepository interface
            return await _data.Persons.Get(personById)
                .ToResultAsync(new PersonNotFoundFailure())
                .SelectAsync(p => new PersonDto(p.Name, p.Email));
        }

        private readonly CompanyData _data;
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
    public class PersonDto
    {
        public PersonDto(string name, string email)
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
            CreateRule(new Failure("Id is required.", "id"))
                .InvalidWhen(request => string.IsNullOrWhiteSpace(request.Id));
        }
    }

    // Log your event. See below for more on this.
    public class Logger : FallibleEventLogger<Request, PersonDto>
    {
        public Logging(ILoggingService logger) : base(logger) { }

        // Always log the incoming request
        public override void OnBoth(Request request, IResult<PersonDto> result)
        {
            _logger.Info($"RequestId: {request.Id}");
        }

        // Log the email of the person on success
        public override void OnSuccess(Request request, PersonDto response)
        {
            _logger.Info($"Found: {response.Email}");
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
        CreateRule(new Failure("Name is required.", "name")
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        // dynamic messages
        CreateRule(p => new Failure($"Name cannot be {p.Name}.", nameof(p.Name).ToLower()))
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.Name));

        CreateRule(new Failure("Phone number is required for employees with phones.", "phone"))
            // InvalidWhens are OR'd.
            // Any of the invalid conditions can invalidate the entity.
            .InvalidWhen(entity => string.IsNullOrWhitespace(entity.PhoneNumber))
            .InvalidWhen(entity => !PhoneIsValidFormat(entity.PhoneNumber))

            // ExceptWhens are OR'd.
            // Any suppression condition that is true will suppress the invalid message.
            .ExceptWhen(entity => !entity.IsEmployee)
            .ExceptWhen(entity => !entity.HasPhone);
    }
}
```

### Event Post Processors

Post Processors fire after the handling of an event or after validation failure. Post Processors should not change the response of the event; however, they can fire off commands such as notifications and logging.

* Inherit from PostProcessorAbstract to define different processing for three channels of the IResult<Response>: Success, Fail, Both.
* Inherit from IPostProcessor for a single channel that fires on both types of Result.

There are default logging Post Processor implementations in the VoidCore.Model library. See above for an example.

### Workflow State Management

Using a finite state machine, control valid transitions between states in a workflow.

```csharp
public partial class Workflow : WorkflowAbstract<Workflow.State, Workflow.Command>
{
    // Build valid state transitions using fluent syntax
    public Workflow() : base(optionsBuilder =>
        optionsBuilder
            // Not Started
            .AddTransition(State.NotStarted, Command.Start, State.ApprovalRequested)
            .AddTransition(State.NotStarted, Command.Cancel, State.Cancelled)

            // Approval Requested
            .AddTransition(State.ApprovalRequested, Command.Approve, State.Approved)
            .AddTransition(State.ApprovalRequested, Command.Reject, State.NotStarted)
            .AddTransition(State.ApprovalRequested, Command.Cancel, State.Cancelled)

            // Approved
            .AddTransition(State.Approved, Command.Revoke, State.Revoked)
            .AddTransition(State.Approved, Command.Expire, State.Expired))
    { }

    public enum State { NotStarted, ApprovalRequested, Approved, Cancelled, Revoked, Expired }

    public enum Command { Start, Approve, Reject, Cancel, Revoke, Expire }

    private IResult<State> MoveNext(Request request, Command command)
    {
        return GetNext(request.CurrentState, command)
            .TeeOnSuccess(newState => request.CurrentState = newState);
    }
}

// Using the move next command returns a result of if the state was successfully changed on the request.
var result = workflow.MoveNext(request, Workflow.Command.Approve)
    .TeeOnSuccess(AddApproval);
```
