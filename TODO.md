# TODO: v18.0 review handoff

Findings from a code review of `dev` vs `main` (103 files, v18.0 hardening release). Build is clean and all 494 tests pass. No blocking bugs were found. Items below are follow-ups and decisions, split into fixes that are ready to apply and design calls for the API cleanup.

Decision on breaking changes: not a major concern for 18.0, but weigh each one and document it in `docs/upgrades.md`.

## Ready to apply

### 1. `PaginationOptions` regression (introduced this round)

File: `src/VoidCore.Model/Responses/Collections/PaginationOptions.cs`

* The constructor clamps `Take = Math.Max(1, take)`. `Take = 0` is intentionally allowed to query an empty page, so this is a regression. Change to `Math.Max(0, take)`. `GetPage` then yields an empty page (`Skip(n).Take(0)`).
* `Skip` repeats `Math.Max(1, Page)`, which the constructor already guarantees. Remove the duplicate.
* `Skip` is public API only used by `GetPage` in `EnumerableExtensions.cs`. Consider making it `internal`.
* Update the doc comment ("page and the take are at least 1"), the `docs/upgrades.md` entry, and `PaginationOptionsTests` to cover `Take = 0`.

### 2. `MaxSchedulePeriods` comment and docs overclaim

Files: `src/VoidCore.Finance/Financial.cs`, `AmortizationRequest.cs`, `InvestmentRequest.cs`, `docs/upgrades.md` (and `docs/finance.md` if mentioned)

* `MaxSchedulePeriods = 10_000_000` is about 1 GB of `AmortizationPeriod` objects (roughly 90 bytes each), and is still reachable with a rate of 0. It does not protect against untrusted input.
* Reword the comment, `<exception>` docs and upgrade note to call it a sanity ceiling only. Requests should be validated in the domain application.
* Optionally lower the cap, but the wording is the required fix.

### 3. `GetSafeFileName` whitespace replacement

File: `src/VoidCore.Model/Text/TextHelpers.Files.cs`

* A whitespace-only `replacement` (for example `" "`) can still produce a blank result through the fallback.
* Non-breaking fix: `var fallback = string.IsNullOrWhiteSpace(replacement) ? "_" : replacement;`. The method then never returns blank, so consumers don't need to check. Don't throw, because that would be a large breaking change.
* Add a test.

### 4. Avoid boxing in `RespondedWithEntityMessage`

Files: `src/VoidCore.Model/Responses/ResponseLogMessages.cs`, `Messages/EntityMessageEventLogger.cs`

* The log method takes `object? entityId`, which boxes value-type ids on every call.
* Make it generic, `RespondedWithEntityMessage<TId>(ILogger, string message, TId entityId)`. `EntityMessageEventLogger<TRequest, TId>` already has `TId`.
* Not breaking: the class is `internal` and the log output is identical.

## Design decisions (fold into API cleanup)

### 5. `/api/{**path}` 404 catch-all

File: `src/VoidCore.AspNet/Routing/RoutingApplicationBuilderExtensions.cs`

* Purpose: stop `GET /api/typo` from returning the SPA HTML with a 200, and `POST /api/typo` from returning a 400 antiforgery failure.
* Caveats: `endpoints.Map` matches every HTTP method, so a real controller route hit with the wrong verb returns 404 instead of 405. This isn't documented.
* FoodStuffs and HomeSensors use only the `ApiRouteAttribute.BasePath` constant in `[Route(...)]`. They don't use the `ApiRouteAttribute` class itself. Exception handling is already done by the global `ApiRouteExceptionFilterAttribute` (registered in `RoutingServiceCollectionExtensions`) plus `UseSpaExceptionPage` for non-API requests.
* The catch-all isn't exception handling, so removing it brings the SPA-HTML-for-bad-API-URL bug back.
* Recommended alternative: remove the extra endpoint and make the SPA fallback skip `/api` instead. Use `MapFallback` with a predicate or an `IsApiRequest` check. This is cleaner and avoids the 405 issue. Update the `SpaPipelineTests` and the upgrade note if changed.

### 6. Health endpoint detail defaults are inconsistent

File: `src/VoidCore.AspNet/Health/HealthCheckEndpointExtensions.cs`

* `MapHealthEndpoint(pattern, includeDetails: false)` hides descriptions and exception messages by default, but the public `WriteResponseAsync(context, report)` still includes them. Anyone wiring it directly keeps the leak.
* Both FoodStuffs and HomeSensors call plain `MapHealthEndpoint()` (`Program.cs`), so they silently lose details after this change.
* Recommendation:
  * Make `WriteResponseAsync(context, report)` hide details by default too.
  * Add an overload taking `Func<HttpContext, bool> includeDetails` so apps can show details to authenticated users only, for example `ctx => ctx.User.Identity?.IsAuthenticated == true`. Keep the `bool` overload as a convenience.
  * `.RequireAuthorization()` already works on the returned builder.
* Alternative to weigh: move the endpoint into the apps themselves. Keeping it in VoidCore with the flexible overload is preferred.

### 7. `ToItemSet` has no ordering check (parity with `ApplyEfSpecification`)

File: `src/VoidCore.EntityFramework/QueryableExtensions.cs`

* `ApplyEfSpecification` throws when paging is enabled without an ordering. `QueryableExtensions.ToItemSet` has no equivalent, so unordered pages can repeat or skip rows.
* Detection is possible. Don't rely on casting to `IOrderedQueryable<T>`, because EF's query type always implements it. Use an `ExpressionVisitor` over `query.Expression` that looks for `Queryable.OrderBy`, `OrderByDescending`, `ThenBy` or `ThenByDescending`. It still works after a `Select` projection (for example FoodStuffs `ListCategoriesHandler`).
* Risk: consumers calling `ToItemSet` without ordering would start throwing. FoodStuffs `ListDeletedRecipesHandler` orders correctly, but not every FS and HS call site has been checked.
* Options: throw now, or log a warning in 18.x and throw in the next major.

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
