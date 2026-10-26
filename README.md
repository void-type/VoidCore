# VoidCore

[![License](https://img.shields.io/github/license/void-type/VoidCore.svg)](https://github.com/void-type/VoidCore/blob/main/LICENSE.txt)
[![Build Status](https://github.com/void-type/VoidCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/void-type/VoidCore/actions/workflows/ci.yml)

[VoidCore](https://github.com/void-type/voidcore) is a set of core libraries for building business applications on .NET.

[FoodStuffs](https://github.com/void-type/foodstuffs) is a comprehensive example of how applications can be built using VoidCore.

## Documentation

VoidCore is split into packages so you only get what you need.

| Docs | Description | Release |
| --- | --- | --- |
| [VoidCore.Model](docs/model.md) | An opinionated core for building business applications. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Model.svg)](https://www.nuget.org/packages/VoidCore.Model/) |
| [VoidCore.AspNet](docs/aspnet.md) | Configure ASP.NET web applications based on VoidCore.Model. Includes single-page front end support. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/) |
| [VoidCore.EntityFramework](docs/entityFramework.md) | Entity Framework Core data access for applications based on VoidCore.Model. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/) |
| [VoidCore.Finance](docs/finance.md) | A financial library with primary focus on simple amortization and time-value of money. | [![NuGet package](https://img.shields.io/nuget/v/VoidCore.Finance.svg)](https://www.nuget.org/packages/VoidCore.Finance/) |

## Upgrading

### Upgrading to 17.0

Audit and soft-delete timestamps are now always **UTC `DateTimeOffset`** values, and time is supplied by .NET's built-in `TimeProvider`. There is one convention and one set of types.

#### Removed

* `IDateTimeService`, `NowDateTimeService`, `UtcNowDateTimeService`, `DiscreteDateTimeService`. Use `TimeProvider` instead.
* `IAuditableWithOffset` and `ISoftDeletableWithOffset`. `IAuditable` and `ISoftDeletable` now use `DateTimeOffset`.
* `AddAuditabilityWithOffset` and `AddSoftDeletabilityWithOffset`. Use `AddAuditability` and `AddSoftDeletability`.
* The `DateTime` overloads of `SetAuditCreated`, `SetAuditModified`, and `SetSoftDeleted`.

#### Changed

* `AddAuditability`, `AddSoftDeletability`, and `SetAuditableProperties` take a `TimeProvider` instead of an `IDateTimeService`.
* `IAuditable.CreatedOn`/`ModifiedOn` and `ISoftDeletable.DeletedOn` are `DateTimeOffset` (`DeletedOn` is nullable).
* `SetAuditableProperties` previously passed a `DateTime` to entities implementing `IAuditableWithOffset`. It now sets the correct UTC value.
* `ICurrentUserAccessor` has a new synchronous `string GetUserName()`. Custom implementations must add it. It should return the same name as `GetUser().Login` without evaluating policies.

#### Added

* `AuditableSaveChangesInterceptor` and `SoftDeletableSaveChangesInterceptor` in `VoidCore.EntityFramework` set audit and soft-delete properties for both `SaveChanges` and `SaveChangesAsync`. They replace overriding `SaveChanges` in your DbContext. See [Entity Framework](docs/entityFramework.md).
* `ApplySoftDeleteQueryFilter()` hides soft-deleted entities from queries. `IncludeSoftDeleted()` and `OnlySoftDeleted()` bypass that filter, for example for a recycle bin.
* `SoftDeletableSaveChangesInterceptor.RequireRecycleBin` makes soft-deleting before permanently deleting mandatory.
* `MapHealthEndpoint()` in `VoidCore.AspNet` maps a `/health` endpoint that returns 200 or 503 with each check's description and exception message in a JSON body. See [ASP.NET](docs/aspnet.md).

#### Steps

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

## Developers

VoidCore is built on [GitHub Actions](https://github.com/void-type/VoidCore/actions/workflows/ci.yml) and released on [Nuget](https://www.nuget.org/packages?q=voidcore&prerel=false). To release, run the CI workflow manually on main. It builds, pushes to NuGet using trusted publishing, then tags and creates the GitHub release.

To work on VoidCore, you will need the [.NET SDK](https://dotnet.microsoft.com/download) and [PowerShell](https://github.com/PowerShell/PowerShell/releases/latest).

See the /build folder for scripts used to test and build this project. Run build.ps1 to make a production build.

```powershell
./build/build.ps1
```

There are [VSCode](https://code.visualstudio.com/) tasks for each script. The build task (ctrl + shift + b) performs the standard CI build.
