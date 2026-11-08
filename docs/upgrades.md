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
* `PaginationOptions` now corrects out-of-range input instead of passing it to the query: `Page` is at least 1 and `Take` is between 1 and the new `PaginationOptions.MaxTake` (1000). Previously a page of 0 or a huge page could produce a negative or overflowed offset, and a huge `Take` an unbounded query. To get an entire set, disable pagination instead of using a large `Take`. The new `PaginationOptions.Skip` gives the overflow-safe offset.
* `EfReadOnlyRepository.Count` now counts all entities matching the specification and ignores its pagination. Previously a paged specification returned the size of the page.
* `ApplyEfSpecification` (and so every EF repository query) throws `InvalidOperationException` if a specification enables pagination without an ordering, because unordered pages can repeat or skip rows. Add an ordering that ends with a unique property. Counting is unaffected.
* Saving a modified entity that implements `IAuditable` no longer overwrites `CreatedOn` and `CreatedBy`. Previously updating a detached entity (for example `Update` with an object mapped from a DTO) replaced them with the DTO's default values.
* `SplitDateRangeIntoIntervals` now validates its arguments immediately: it throws `ArgumentOutOfRangeException` for an interval of less than 1 day (previously 0 days looped forever and negative values misbehaved) and for an undefined `OverlapMitigation` (previously only thrown once enumerated). It no longer throws near `DateTime.MaxValue`.
* `DateTimeRange.StartDate` and `EndDate` are get-only instead of `init`. `with` expressions and object initializers that set them no longer compile, because they could produce a range that starts after it ends.
* Security headers (`UseXFrameOptions`, `UseReferrerPolicy`, the Cross-Origin headers, `UseContentSecurityPolicy` and the rest) now replace an existing header instead of appending. Previously a response re-executed by `UseStatusCodePagesWithReExecute` (every non-API 404 or 403 page in `UseSpaExceptionPage`) got duplicated values such as `X-Frame-Options: DENY, DENY`, which browsers can ignore.
* `UseReportingEndpoints` takes an `IReadOnlyDictionary<string, string>` (name to one absolute URL) instead of `Dictionary<string, List<string>>`. The old header quoted the names and repeated them for multiple URLs, which isn't valid, so browsers dropped it. Names and URLs are validated at startup.
* `UseSpaEndpoints` maps unmatched requests under `/api` to a 404 instead of the SPA fallback. Previously `GET /api/typo` returned the SPA HTML with a 200 and `POST /api/typo` a 400 antiforgery failure.
* `HttpResponder.Respond(IResult)` now returns an empty 200 on success as documented. It was returning 204 because ASP.NET turns a null `ObjectResult` into No Content. A null success value in `Respond<T>` still returns 204.
* `MapHealthEndpoint` no longer includes each check's description and exception message in the JSON body by default, because they can expose server and database details to anyone who can reach the endpoint. Pass `includeDetails: true` to restore them when the endpoint is only reachable by trusted monitors. `WriteResponseAsync(context, report)` still includes them.
* `TextHelpers.Link` HTML-encodes the caption and URL and throws `ArgumentException` for a URL scheme other than http, https, mailto, or tel (such as `javascript:`). If you passed HTML as the caption, it is now shown as text.
* `GetSafeFileName` now also replaces `:` and other characters that are only invalid on Windows, so a name is the same on every platform, repeated `..` left behind by a replacement, and trailing dots and spaces. Names Windows reserves for devices (`CON`, `NUL`, `COM1`, with or without an extension) are prefixed with the replacement, and an empty result becomes the replacement. It throws `ArgumentException` for a replacement that contains invalid characters or `..`. Use it on the file name only. `GetSafeFilePath` is not a sandbox: it leaves rooted paths intact, so check the path starts with your expected folder.
* `GetTypeNameWithoutEnding` never strips a name to an empty string. For example `Settings` with the ending `settings` stays `Settings`.
* `AutoPolicyAuthorizeAttribute` only removes a trailing `Only` from the class name, so `ReadOnlyUser` is the policy `ReadOnlyUser` instead of `Read`, and a class named just `Only` throws instead of silently using the default policy. Classes that already end in `Only` are unaffected.
* `Email`, `EmailOptionsBuilder.SetSubject` and `AddRecipient` throw `ArgumentException` for a subject or recipient containing a line break (header injection) and for a blank recipient. `Email` copies its recipients.
* `HttpResponder.RespondWithInlineFile` only shows images (except SVG), audio, video, PDF, and plain text inline. Other types, such as HTML and SVG, are sent as downloads so a user-supplied file can't run script on your site. `RespondWithFile` sends `application/octet-stream` instead of the non-standard `application/force-download`.
* `AddSpaSecurityServices` now wraps `IAntiforgery` so antiforgery (CSRF) validation failures are logged at Warning level with the request method, path and reason. ASP.NET Core only logs them at Information, which production log levels usually filter out, leaving rejected requests (HTTP 400) unexplained. If you register your own `IAntiforgery` after calling `AddSpaSecurityServices`, it replaces this wrapper.

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
