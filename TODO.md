# TODO: v18.0 review handoff

Findings from a code review of `dev` vs `main` (103 files, v18.0 hardening release). Build is clean and all 518 tests pass. No blocking bugs were found. Items 1 to 7 are all done in VoidCore. The remaining work is trying the `[ApiController]` conventions on an app (item 5) and the broader API cleanup. Review items that weren't acted on (breaking-change notes, unreviewed files) are at the bottom.

Decision on breaking changes: not a major concern for 18.0, but weigh each one and document it in `docs/upgrades.md`.

## Ready to apply

### 1. `PaginationOptions` regression (introduced this round) — DONE

File: `src/VoidCore.Model/Responses/Collections/PaginationOptions.cs`

* Fixed: `Take = Math.Max(0, take)` so `Take = 0` still queries an empty page. The duplicate `Math.Max(1, Page)` in `Skip` was removed. `Skip` stays public on purpose in case a consumer uses it. Docs, upgrade note and tests updated.

### 2. `MaxSchedulePeriods` comment and docs overclaim — DONE

Resolved by removing the cap entirely (commit `7c45b0b`). Requests should be validated in the domain application.

Follow-up in PaymentCalculator — DONE (uncommitted in `D:\code\PaymentCalculator`, verified against a local VoidCore 18 pack, 41 tests pass): both handlers now also catch `ArgumentOutOfRangeException` and return the overflow failure, `CalculateLoan.RequestValidator` rejects an annual rate of -100% or less, and `CalculateInvestment.CreateRequest` returns a failure for the same. Its `Directory.Packages.props` still pins VoidCore 17.0.2 (tests were only run against a local 18 pack); bump to 18.x once published.

### 3. `GetSafeFileName` whitespace replacement — DONE

File: `src/VoidCore.Model/Text/TextHelpers.Files.cs`

* Fixed without throwing: the fallback is the replacement trimmed of dots and spaces, or `_` when nothing is left. This also fixes a `"."` replacement returning `"."`. Docs, upgrade note and tests updated.

### 4. Avoid boxing in `RespondedWithEntityMessage` — DONE

Files: `src/VoidCore.Model/Responses/ResponseLogMessages.cs`, `Messages/EntityMessageEventLogger.cs`

* The source generator rejects generic log methods (SYSLIB1011), so the method is now generic by hand: a `LoggerMessage.Define<string, TId>` delegate cached per id type in a nested generic class, guarded by `IsEnabled`. It reuses the generated event ID and name, so log output is identical. Not breaking (`internal`). Tests added.

## Design decisions (fold into API cleanup)

### 5. `/api` 404 catch-all and `[ApiController]` adoption — DONE in VoidCore, needs an app trial

Files: `src/VoidCore.AspNet/Routing/RoutingApplicationBuilderExtensions.cs`, `docs/upgrades.md` ("Recommended app changes"), `tests/VoidCore.Test/AspNet/Routing/SpaPipelineTests.cs`

* The extra `/api/{**path}` endpoint is gone. `UseSpaEndpoints` now drops the SPA fallback endpoint for `/api` requests, so unmatched API requests get the framework's empty 404 and `/apiary` still reaches the SPA. A wrong-verb request to a real API route is also a 404 (the fallback wins candidate selection, then gets dropped), not a 405.
* `ApiRouteExceptionFilterAttribute` and `AddApiExceptionFilter()` are removed. `UseSpaExceptionPage` handles API exceptions in middleware and returns a 500 `ProblemDetails` (generic title and `traceId`; exception in `detail` in development). The framework logs the exception at Error. Page requests are unchanged.
* Experiment findings that drove this (verified on ASP.NET Core 10): without `[ApiController]` a bad query int silently binds as 0, and a bad JSON body gives the action a `null` model (the source of the vague 500s). `[ApiController]` turns both into a 400 `ValidationProblemDetails` naming the field. It also converts empty error results to `ProblemDetails`. It does not touch domain failures from `HttpResponder` (400 with a body). It does not handle exceptions.
* Still to do: try the "Recommended app changes" steps in `docs/upgrades.md` on FoodStuffs or HomeSensors (remove `AddApiExceptionFilter`, change `ApiRouteAttribute.BasePath` to `ApiRoutes.BasePath`, add `[ApiController]`, drop inferred `[From*]` attributes, update the client to handle `application/problem+json` for 400 and 500). Both apps currently call `AddApiExceptionFilter()` and will fail to compile until it's removed.
* `ApiRouteAttribute` is removed (nothing used the class). `BasePath` and `IsApiRequest` moved to the static `ApiRoutes` class. Consumers change `ApiRouteAttribute.BasePath` to `ApiRoutes.BasePath` (FoodStuffs and HomeSensors controllers, mechanical). Consider an `[ApiController]` base class in VoidCore if you want it applied in one place.
* Test note: the `IsSpaFallback` check relies on the fallback being the only `Home.Index` endpoint without an attribute route. It is covered by `SpaPipelineTests`.

### 6. Health endpoint detail defaults are inconsistent — DONE

File: `src/VoidCore.AspNet/Health/HealthCheckEndpointExtensions.cs`

* `WriteResponseAsync(context, report)` now hides details by default, matching `MapHealthEndpoint()`.
* New `MapHealthEndpoint(Func<HttpContext, bool> includeDetails, string pattern = "/health")` decides per request (for example `ctx => ctx.User.Identity?.IsAuthenticated == true`). The `bool includeDetails` overload stays as a convenience and delegates to it. `WriteResponseAsync` has matching `bool` and function overloads. `.RequireAuthorization()` still works on the returned builder.
* Both FoodStuffs and HomeSensors call plain `MapHealthEndpoint()`, so they silently lose details (the safe default). If their monitors need them, pass `includeDetails: true` or a function.
* Docs, upgrade note and tests updated. The endpoint stays in VoidCore.

### 7. `ToItemSet` ordering check (parity with `ApplyEfSpecification`) — DONE

File: `src/VoidCore.EntityFramework/QueryableExtensions.cs`

* `ToItemSet` throws `InvalidOperationException` when paging is enabled and the query has no ordering. It walks the call chain of `query.Expression` (through `Where`, `Select`, `Include`, `TagWith`, `AsNoTracking` and so on) for `OrderBy`, `OrderByDescending`, `ThenBy` or `ThenByDescending`. An ordering inside a lambda (a sorted subquery in a projection) doesn't count. Paging disabled is unaffected. Throws now, no warning phase.
* Checked FoodStuffs: every `ToItemSet` on an `IQueryable` goes through `ApplyEfSpecification` (already enforced) or orders explicitly (`ListMealPlansHandler`, `ListDeletedRecipesHandler`). HomeSensors has no such calls. Note: `ListMealPlansHandler` orders by `CreatedOn` only. That passes the check, but add a unique tie-breaker such as `ThenBy(Id)` if rows can share a timestamp.

## Notes on breaking changes in this round

All are documented in `docs/upgrades.md` and covered by the 17.0 to 18.0 bump:

* protected field to property changes (`EventPipelineAbstract.InnerHandler`, `RequestLoggerAbstract.Logger`)
* `DateTimeRange.StartDate` and `EndDate` are get-only instead of `init`
* `UseReportingEndpoints` signature change
* new `ValueObject` comparison operators
* security headers now replace instead of append

## Not reviewed in detail

`.globalconfig`, `.editorconfig`, `README.md`, `build/build.ps1`, and `docs/*.md` edits other than `docs/upgrades.md`.

## Looks good (no action)

* Antiforgery logging wrapper: idempotent, handles instance, factory and type descriptors.
* `AddServicesFromAssembliesAsBaseType` closed and open generic handling.
* `Link` and `HttpResponder` inline-file hardening.
* Amortization underpayment handling (capitalized interest, `TotalPaid` from actual payments).
