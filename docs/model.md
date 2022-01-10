# VoidCore.Model

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg)](https://www.nuget.org/packages/VoidCore.Model/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.Model.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.Model)

## Installation

```powerShell
dotnet add package VoidCore.Model
```

## Features

An opinionated core for building business applications.

### API Responses

Make predictable data APIs.

* Standardized failure objects with user message and optional UI handle for field highlighting.
* User messages (with optional Entity ID for standard CRUD operations).
* Paginated item sets.
* Simple files.

### Data Persistence

This service interface is designed to quickly implement a data layer with an expanded feature set.

* Asynchronous repositories with read/write control and specification-based queries.
* Specifications include the most common LINQ functions, except Select.
* Soft delete on entities. Will mark them with a deleted date. Use a specification to ensure they aren't included in queries.
* Auditable entities via Created and Modified names/dates.
* Easy pagination of queried data sets.

If you need more flexibility, it's recommended to create a service for one-off use-cases or just use DbContext directly.

### Emailing

* Service interface for sending emails from the domain layer.
* Template emails using the builder pattern. Templates can be realized with html or text formatting.

### Common Service Interfaces

Interfaces for other common services that the domain can use.

* Time
* Current user

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

Using these functions and those below we can take code that looks like this:

```csharp
public override async Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
{
    var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

    var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken);

    if (maybeRecipe.HasNoValue)
    {
        return Fail(new RecipeNotFoundFailure());
    }

    var recipe = maybeRecipe.Value;

    await RemoveImages(recipe, cancellationToken);
    await _data.CategoryRecipes.RemoveRange(recipe.CategoryRecipes, cancellationToken);
    await _data.Recipes.Remove(recipe, cancellationToken);

    return Ok(EntityMessage.Create("Recipe deleted", recipe.Id));
}
```

And turn it into something more concise and with fewer intermediate variables:

```csharp
public override Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
{
    return _data.Recipes.Get(byId, cancellationToken)
        .ToResultAsync(new RecipeNotFoundFailure())
        .TeeOnSuccessAsync(r => RemoveImages(r, cancellationToken))
        .TeeOnSuccessAsync(r => _data.CategoryRecipes.RemoveRange(r.CategoryRecipes, cancellationToken))
        .TeeOnSuccessAsync(r => _data.Recipes.Remove(r, cancellationToken))
        .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
}
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
        return Result.Fail<Person>(new Failure("Person is not found.", "personIdField"));
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

    // Then is called "bind" in other programming languages. We can call another result-returning function without wrapping it a nested result.
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
        return Result.Fail<Person>(new PersonNotFoundFailure());
    }

    return Result.Ok(maybePerson.Value);
}
```

There are useful extension methods to make pipelines of Maybes and even convert them to Results. There are also async variants of all extensions.

```csharp
// Create a maybe from anything.
var maybePerson = Maybe.From(_data.Persons.GetById(id))

    // Filter on a predicate. A maybe that doesn't match will be replaced with Maybe.None<Person>().
    .Where(p => p.Name == "Patrick Stewart")

    // Then will bind your maybe into another maybe-returning function.
    // This allows you to use the output of one maybe function as input for another and prevents nested maybes.
    .Then(p => _data.Actors.GetFromPerson(p))

    // Safe mappings. If there is no value, it will return a Maybe.None<Person>().
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

Below is an example of using an event within an ASP.NET MVC controller.

```csharp
public class PersonsController : Controller
{
    // See Pipelines at the end of the full example for a way to simplify the controller constructor and methods.
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

            // Persons.Get returns a Maybe<Person> from the VoidCore.Model.Data.IReadOnlyRepository interface
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

    // Log your request with a RequestLogger. See below for more on this.
    public class RequestLogger : RequestLoggerAbstract<Request>
    {
        public class RequestLogger(ILogger<RequestLogger> logger) : base(logger) {}

        // Always log the incoming request
        public override void Log(Request request)
        {
            Logger.LogInformation("RequestId: {RequestId}", request.Id);
        }
    }

    // Log your event response with a PostProcessor. See below for more on this.
    public class ResponseLogger : FallibleEventLogger<Request, PersonDto>
    {
        public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }

        // Log the email of the person on success. Failures are logged in base.
        public override void OnSuccess(Request request, PersonDto response)
        {
            Logger.LogInformation("Found: {Email}", response.Email);

            // Be sure to always call base methods.
            base.OnSuccess(request, response);
        }
    }

    // Pre-construct your event pipeline using constructor DI. Then the Pipeline can be injected into controllers, simplifying your controller constructor.
    // See the AddDomainEvents IServiceCollection extension method to auto-register all event components within an assembly.
    public class Pipeline : EventPipelineAbstract<Request, PersonDto>
    {
        public Pipeline(Handler handler, RequestValidator validator, RequestLogger requestLogger, ResponseLogger responseLogger)
        {
            InnerHandler = handler
                .AddRequestLogger()
        }
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

        // Dynamic messages
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

### Event Request Loggers and Post Processors

Request loggers fire before validators and handlers. They log information about the request before the other parts of the pipeline have a chance to throw an exception.

* Inherit from RequestLoggerAbstract to use the default MS ILogger.
* Inherit from IRequestLogger to use any other logging implementation.

Post Processors fire after the handling of an event or after validation failure. Post Processors should not change the response of the event; however, they can fire off commands such as notifications and logging.

* Inherit from one of the Response loggers to log standard information about the various built-in response types.
* Inherit from FallibleEventLoggerAbstract to create a logger for a custom response type, such as a DTO.
* Inherit from PostProcessorAbstract to define different processing for three channels of the IResult<Response>: Success, Fail, Both.
* Inherit from IPostProcessor for a single channel always fires. This is good for completely custom logic.

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
