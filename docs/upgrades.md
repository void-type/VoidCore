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
