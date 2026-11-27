# Upgrading

Notes for upgrading between major versions of VoidCore.

## Upgrading to 18.0

Mostly small breaking changes that came out of enabling stricter analyzers. Recompile everything that depends on VoidCore.

### Changed

* `EventPipelineAbstract.InnerHandler` is now a protected property instead of a protected field. Subclasses that assign it compile unchanged, but they must be recompiled.
* `RequestLoggerAbstract.Logger` is now a protected property instead of a protected readonly field. Subclasses must be recompiled.
* `ValueObject` now defines the `<`, `<=`, `>` and `>=` operators. A null value sorts before any non-null value.
* `ValueObject.CompareTo(null)` now returns 1 as `IComparable` requires. Previously it threw a `NullReferenceException`.
* `AddServicesFromAssemblies` and `AddServicesFromAssembliesAsBaseType` now throw `NotSupportedException` instead of `NotImplementedException` for an unsupported `ServiceLifetime`.
* `Slugify` now uses regular expressions with a one-second timeout and throws `RegexMatchTimeoutException` instead of running indefinitely.
* Response loggers (`ItemSetEventLogger`, `UserMessageEventLogger`, `EntityMessageEventLogger`, `SimpleFileEventLogger`, `FallibleEventLoggerAbstract`, `GetWebClientInfo.ResponseLogger`) and `RequestLoggingScopeMiddleware` now use source-generated logging. Message templates are unchanged, but the log entries now have event IDs and names.
* `VoidCore.Model` and `VoidCore.EntityFramework` now use `ConfigureAwait(false)` consistently. Code that depended on resuming on a captured synchronization context after calling these libraries must marshal back to it explicitly. `VoidCore.AspNet` is unaffected because ASP.NET Core has no synchronization context.
* `PaginationOptions` now corrects out-of-range input instead of passing it to the query: `Page` is at least 1 and `Take` is at least 0 (a `Take` of 0 still queries an empty page). Previously a page of 0 or a huge page could produce a negative or overflowed offset. There is no upper limit on `Take`, so validate it if it comes from untrusted input. The new public `PaginationOptions.Skip` gives the overflow-safe offset.
* `EfReadOnlyRepository.Count` now counts all entities matching the specification and ignores its pagination. Previously a paged specification returned the size of the page.
* `ApplyEfSpecification` (and so every EF repository query) throws `InvalidOperationException` if a specification enables pagination without an ordering, because unordered pages can repeat or skip rows. Add an ordering that ends with a unique property. Counting is unaffected. The `ToItemSet` extension on `IQueryable<T>` (VoidCore.EntityFramework) has the same rule: it throws when paging is enabled and the query has no `OrderBy`, `OrderByDescending`, `ThenBy` or `ThenByDescending` in its chain (an ordering inside a lambda, like a sorted subquery in a projection, doesn't count). Queries with paging disabled are unaffected.
* Saving a modified entity that implements `IAuditable` no longer overwrites `CreatedOn` and `CreatedBy`. Previously updating a detached entity (for example `Update` with an object mapped from a DTO) replaced them with the DTO's default values.
* `SplitDateRangeIntoIntervals` now validates its arguments immediately: it throws `ArgumentOutOfRangeException` for an interval of less than 1 day (previously 0 days looped forever and negative values misbehaved) and for an undefined `OverlapMitigation` (previously only thrown once enumerated). It no longer throws near `DateTime.MaxValue`.
* `DateTimeRange.StartDate` and `EndDate` are get-only instead of `init`. `with` expressions and object initializers that set them no longer compile, because they could produce a range that starts after it ends.
* Security headers (`UseXFrameOptions`, `UseReferrerPolicy`, the Cross-Origin headers, `UseContentSecurityPolicy` and the rest) now replace an existing header instead of appending. Previously a response re-executed by `UseStatusCodePagesWithReExecute` (every non-API 404 or 403 page in `UseSpaExceptionPage`) got duplicated values such as `X-Frame-Options: DENY, DENY`, which browsers can ignore.
* `UseReportingEndpoints` takes an `IReadOnlyDictionary<string, string>` (name to one absolute URL) instead of `Dictionary<string, List<string>>`. The old header quoted the names and repeated them for multiple URLs, which isn't valid, so browsers dropped it. Names and URLs are validated at startup.
* `UseSpaEndpoints` no longer sends unmatched requests under `/api` to the SPA fallback. They get the framework's empty 404, including a request with the wrong method for an existing route (not a 405). Previously `GET /api/typo` returned the SPA HTML with a 200 and `POST /api/typo` a 400 antiforgery failure. Requests such as `/apiary` still fall back to the SPA.
* **Removed** `ApiRouteAttribute` (`[ApiRoute("x")]`). Nothing used it except for the constant. Use `[Route(ApiRoutes.BasePath + "/x")]`, or just `[Route("api/x")]` with the prefix matching `ApiRoutes.BasePath`. The `BasePath` constant and `IsApiRequest(HttpContext)` moved to the new static class `ApiRoutes` in the same namespace, so `ApiRouteAttribute.BasePath` becomes `ApiRoutes.BasePath`.
* **Removed** `ApiRouteExceptionFilterAttribute` and `AddApiExceptionFilter()`. `UseSpaExceptionPage` now handles API exceptions in middleware, so it also covers exceptions outside of controller actions. An unhandled exception under `/api` returns a 500 `ProblemDetails` (`application/problem+json`: `status`, `title`, `type` and a `traceId`) instead of a `UserMessage`. In development the exception text is in `detail`, which matches the old behavior of putting it in the message. The exception is logged at Error by the framework. Remove the `AddApiExceptionFilter()` call, and update clients that read `message` from a 500 response (see "Recommended app changes" below).
* `HttpResponder.Respond(IResult)` now returns an empty 200 on success as documented. It was returning 204 because ASP.NET turns a null `ObjectResult` into No Content. A null success value in `Respond<T>` still returns 204.
* `MapHealthEndpoint` no longer includes each check's description and exception message in the JSON body by default, because they can expose server and database details to anyone who can reach the endpoint. `WriteResponseAsync(context, report)` hides them by default as well, so the two are consistent. To include them, pass `includeDetails: true` when the endpoint is only reachable by trusted monitors, or use the new overload that takes a function to decide per request, for example `MapHealthEndpoint(context => context.User.Identity?.IsAuthenticated == true)` to show details to signed-in users only. `WriteResponseAsync` also has `bool` and function overloads for `includeDetails`. Requiring authorization on the endpoint still works through the returned builder (`.RequireAuthorization()`).
* `TextHelpers.Link` HTML-encodes the caption and URL and throws `ArgumentException` for a URL scheme other than http, https, mailto, or tel (such as `javascript:`). If you passed HTML as the caption, it is now shown as text.
* `GetSafeFileName` now also replaces `:` and other characters that are only invalid on Windows, so a name is the same on every platform, repeated `..` left behind by a replacement, and trailing dots and spaces. Names Windows reserves for devices (`CON`, `NUL`, `COM1`, with or without an extension) are prefixed with the replacement, and an empty result becomes the replacement (or `_` when the replacement is blank or only dots and spaces, so the result is never blank or a dot name). It throws `ArgumentException` for a replacement that contains invalid characters or `..`. Use it on the file name only. `GetSafeFilePath` is not a sandbox: it leaves rooted paths intact, so check the path starts with your expected folder.
* `GetTypeNameWithoutEnding` never strips a name to an empty string. For example `Settings` with the ending `settings` stays `Settings`.
* `AutoPolicyAuthorizeAttribute` only removes a trailing `Only` from the class name, so `ReadOnlyUser` is the policy `ReadOnlyUser` instead of `Read`, and a class named just `Only` throws instead of silently using the default policy. Classes that already end in `Only` are unaffected.
* `Email`, `EmailOptionsBuilder.SetSubject` and `AddRecipient` throw `ArgumentException` for a subject or recipient containing a line break (header injection) and for a blank recipient. `Email` copies its recipients.
* `HttpResponder.RespondWithInlineFile` only shows images (except SVG), audio, video, PDF, and plain text inline. Other types, such as HTML and SVG, are sent as downloads so a user-supplied file can't run script on your site. `RespondWithFile` sends `application/octet-stream` instead of the non-standard `application/force-download`.
* `AmortizationCalculator` handles a payment smaller than the interest due (for example a missed payment through `AmortizationPaymentModification`) as a real loan does: only what was paid counts as interest, the unpaid interest is added to the balance, and the new `AmortizationPeriod.InterestCapitalized` reports it. Previously the interest was charged but never added to the balance and `TotalPaid` counted it as paid (a loan with one missed payment reported a total paid about 90 more than the sum of the payments). `InterestPayment` is now interest actually paid, `TotalInterestPaid` and `TotalPaid` are totals of cash actually paid, and a schedule with underpayments can end with a balance left. The last payment of a loan now closes out the sub-cent rounding residue of the payment calculation, so `BalanceLeft` ends at exactly 0.
* `Financial` functions, `AmortizationRequest` and `InvestmentRequest` throw `ArgumentOutOfRangeException` for a rate of -100% or less, and for a rate and number of periods whose growth can't be represented, instead of a `DivideByZeroException` or `OverflowException`. `Payment` with a rate too small to matter no longer divides by zero. `NumberOfPeriods` throws `ArgumentException` when the payment can never pay off the balance. If you catch `OverflowException` or `DivideByZeroException` around these calls, also catch `ArgumentOutOfRangeException`, or validate the rate (greater than -100%) before building the request.
* `Financial.InterestPayment` and `PrincipalPayment` for the first period of an annuity due (`paymentDueAtBeginningOfPeriod`) now return 0 interest, as Excel and VB do. Previously they returned a negative interest.
* `AddServicesFromAssembliesAsBaseType` registers closed implementations of an open generic base type (such as `IHandler<,>`) against their closed form (`IHandler<A, B>`). Previously it registered them against the open type, which made the service provider throw when it was built. Open generic implementations are only registered against open generic base types, and are skipped for a non-generic base type, which previously threw as well.
* `Result.Fail` enumerates its failures once and copies them, so a lazy sequence isn't evaluated twice and a later change to the caller's array doesn't change the result.
* `Guard.Ensure` with a message builder only builds the message when the condition fails, and `Tee`/`TeeAsync` overloads check their arguments for null like the rest.
* `EventHandlerSyncAbstract.Handle` reports an exception thrown by `HandleSync` through the returned task like an async handler, instead of throwing before the caller can await.
* `Slugify` transliterates letters that have no accent to remove (`ß`, `æ`, `ø`, `ł`, and similar) instead of dropping them, trims leading and trailing hyphens, cuts a single word longer than the maximum length instead of returning an empty slug, and throws `ArgumentOutOfRangeException` for a negative maximum length.
* `PrintObject` throws for a null object immediately, skips indexers, and formats values with the invariant culture.
* The messages of the exceptions thrown for missing settings (`AddSettingsSingleton`) and connection strings (`GetRequiredConnectionString`) name what is missing.
* `EfReadOnlyRepository.ListPage` skips the count query when paging is disabled, and `EfWritableRepository.RemoveRange` is virtual like the other methods.
* `AddSpaSecurityServices` can be called more than once without logging each antiforgery failure twice.
* `AddSoftDeletability` and `AddAuditability` document the order the decorators must be applied in so that soft-deleting also records who modified the entity.
* `AddSpaSecurityServices` now wraps `IAntiforgery` so antiforgery (CSRF) validation failures are logged at Warning level with the request method, path and reason. ASP.NET Core only logs them at Information, which production log levels usually filter out, leaving rejected requests (HTTP 400) unexplained. If you register your own `IAntiforgery` after calling `AddSpaSecurityServices`, it replaces this wrapper.

### Recommended app changes: use the framework's API conventions

VoidCore's API conventions predate `[ApiController]`. These changes are optional, but they remove custom code and give clients a standard error shape. They are verified against ASP.NET Core 10 in the VoidCore tests, but haven't been rolled out to an app yet. Try them on one app first.

**What `[ApiController]` changes**

| Request | Without `[ApiController]` | With `[ApiController]` |
|---|---|---|
| `?page=abc` for an `int` | The action runs with `page = 0` and an invalid `ModelState` | 400 `ValidationProblemDetails` naming `page` |
| JSON body with a wrong type or `null` for a non-nullable property, or malformed or empty JSON | The action runs with a `null` body model, which usually fails with a `NullReferenceException` and a vague 500 | 400 `ValidationProblemDetails` with the field path and reason |
| A non-nullable string property is `null` in the body | The action runs with an invalid `ModelState` | 400 |
| An error status with no body, like `NotFound()` | Empty body | `ProblemDetails` body |
| A missing `int` (query or body) | `0`, no error | `0`, no error. Use `required` or `int?` with a domain rule. |

Domain failures are not affected. `HttpResponder` returns them as a 400 with a body (`IItemSet<IFailure>`), and `[ApiController]` only rewrites results that have no body.

**Steps**

1. Remove `services.AddApiExceptionFilter()`. Keep `services.AddControllers()`. Keep calling `UseSpaExceptionPage(env)` before routing: it now handles API exceptions.
2. Add `[ApiController]` to the API controllers, or to a shared base class. Keep routing them under the shared prefix with `[Route(ApiRoutes.BasePath + "/recipes")]`: `[ApiController]` requires attribute routing and doesn't add the `/api` prefix. Controllers and `UseSpaExceptionPage`/`UseSpaEndpoints` must agree on the prefix, which is why `ApiRoutes.BasePath` (and `ApiRoutes.IsApiRequest`) stay in VoidCore.
3. Remove `[FromBody]`, `[FromQuery]`, `[FromRoute]` and `[FromServices]` where the inferred source is what you want: complex types come from the body, registered services are injected, simple types come from the route or query string. Keep an attribute for anything else, for example a complex type from the query string.
4. Update the client's API error handling to accept three response shapes:
   * A domain failure: 400 with `{ items: [{ message, uiHandle }, ...] }` (unchanged).
   * A binding or validation problem: 400 `application/problem+json` with `errors` (a map of field name to a list of messages). Show these next to the matching inputs when the field names line up. Field names in `errors` are the property path, for example `$.id` for a JSON type error. Always show `title` when there is no `errors`.
   * An unhandled exception: 500 `application/problem+json` with a generic `title` and a `traceId`. Show the `title`, and include the `traceId` so a user can report it. Don't read `message` from a 500 anymore.
5. Decide whether to keep the default 400 for binding errors, or turn it off with `ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)` and validate in your handlers. We recommend keeping it: without it, a bad body arrives as a `null` model.
6. Check that your Swagger or OpenAPI `ProducesResponseType` attributes list the `ProblemDetails` 400 and 500 results where you want them documented.

**Tradeoff:** the wire now has two error shapes for 400: your `IItemSet<IFailure>` for domain failures, and `ProblemDetails` for requests that can't be bound. Clients only need to branch on `Content-Type` (`application/problem+json`).

## Upgrading to 17.0

Audit and soft-delete timestamps are now always **UTC `DateTimeOffset`** values, and time is supplied by .NET's built-in `TimeProvider`. There is one convention and one set of types.

### Removed

* `IDateTimeService`, `NowDateTimeService`, `UtcNowDateTimeService`, `DiscreteDateTimeService`. Use `TimeProvider` instead.
* `IAuditableWithOffset` and `ISoftDeletableWithOffset`. `IAuditable` and `ISoftDeletable` now use `DateTimeOffset`.
* `AddAuditabilityWithOffset` and `AddSoftDeletabilityWithOffset`. Use `AddAuditability` and `AddSoftDeletability`.
* The `DateTime` overloads of `SetAuditCreated`, `SetAuditModified`, and `SetSoftDeleted`.

### Changed

* `AddAuditability`, `AddSoftDeletability`, and `SetAuditableProperties` take a `TimeProvider` instead of an `IDateTimeService`.
* `IAuditable.CreatedOn`/`ModifiedOn` and `ISoftDeletable.DeletedOn` are `DateTimeOffset` (`DeletedOn` is nullable).
* `SetAuditableProperties` previously passed a `DateTime` to entities implementing `IAuditableWithOffset`. It now sets the correct UTC value.
* `ICurrentUserAccessor` has a new synchronous `string GetUserName()`. Custom implementations must add it. It should return the same name as `GetUser().Login` without evaluating policies.

### Added

* `AuditableSaveChangesInterceptor` and `SoftDeletableSaveChangesInterceptor` in `VoidCore.EntityFramework` set audit and soft-delete properties for both `SaveChanges` and `SaveChangesAsync`. They replace overriding `SaveChanges` in your DbContext. See [Entity Framework](entityFramework.md).
* `ApplySoftDeleteQueryFilter()` hides soft-deleted entities from queries. `IncludeSoftDeleted()` and `OnlySoftDeleted()` bypass that filter, for example for a recycle bin.
* `SoftDeletableSaveChangesInterceptor.RequireRecycleBin` makes soft-deleting before permanently deleting mandatory.
* `MapHealthEndpoint()` in `VoidCore.AspNet` maps a `/health` endpoint that returns 200 or 503 with each check's description and exception message in a JSON body. See [ASP.NET](aspnet.md).

### Steps

1. Register a `TimeProvider` in dependency injection.

   ```csharp
   services.AddSingleton(TimeProvider.System);
   ```

2. Replace `IDateTimeService` constructor parameters and fields with `TimeProvider`. `Moment` and `MomentWithOffset` become `GetUtcNow()` (which returns a UTC `DateTimeOffset`).
3. In tests, replace `DiscreteDateTimeService` with `FakeTimeProvider` from the `Microsoft.Extensions.TimeProvider.Testing` package.

   ```csharp
   var timeProvider = new FakeTimeProvider(new DateTimeOffset(2001, 2, 12, 0, 0, 0, TimeSpan.Zero));
   ```

4. Change entity properties from `DateTime` to `DateTimeOffset` (and `DateTime?` to `DateTimeOffset?`). Replace `IAuditableWithOffset`/`ISoftDeletableWithOffset` with `IAuditable`/`ISoftDeletable`.
5. Replace calls to `AddAuditabilityWithOffset` and `AddSoftDeletabilityWithOffset` with `AddAuditability` and `AddSoftDeletability`.
6. Add an EF migration to convert existing `datetime`/`datetime2` columns to `datetimeoffset`. Remove any `HasColumnType("datetime")` mappings on those properties. Existing values that were stored as local time must be converted to UTC during the migration or they will be misread. Values already stored as UTC can be converted directly.

   ```sql
   -- Only if values were stored as local time (adjust the time zone name): convert them to UTC first.
   UPDATE dbo.Recipe SET CreatedOn = CreatedOn AT TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC';

   -- Then convert the column. Existing values are treated as UTC (+00:00).
   ALTER TABLE dbo.Recipe ALTER COLUMN CreatedOn datetimeoffset NOT NULL;
   ```

   Repeat for each audit and soft-delete column.

7. Add `GetUserName()` to any custom `ICurrentUserAccessor` implementation.
8. If your DbContext overrides `SaveChanges` and `SaveChangesAsync` to call `SetAuditableProperties`, remove the overrides and the `TimeProvider` and `ICurrentUserAccessor` constructor parameters. Register the interceptors instead. The interceptors are singletons, so `ICurrentUserAccessor` and `TimeProvider` must be singletons too.

   ```csharp
   services.AddSingleton<AuditableSaveChangesInterceptor>();
   services.AddSingleton<SoftDeletableSaveChangesInterceptor>(); // Only if you use ISoftDeletable.

   services.AddDbContext<MyDbContext>((provider, options) => options
       .UseSqlServer(connectionString)
       .AddInterceptors(
           provider.GetRequiredService<AuditableSaveChangesInterceptor>(),
           provider.GetRequiredService<SoftDeletableSaveChangesInterceptor>()));
   ```

   The `AddAuditability` and `AddSoftDeletability` repository decorators still work for non-EF repositories but shouldn't be combined with the interceptors.
