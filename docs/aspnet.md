# VoidCore.AspNet

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/)

## Installation

```powerShell
dotnet add package VoidCore.AspNet
```

## Features

VoidCore.AspNet is an ASP.NET implementation of VoidCore.Model. It includes helpers for configuring a web application:

* Active Directory group authorization via Windows authentication.
* HTTPS with redirection and HSTS headers.
* Fluent CSP and X-Frame options header configuration.
* Antiforgery for SPAs.
* Exception handling (SPA and MVC) with logging.
  * API endpoint exceptions return a 500 `ProblemDetails` (`application/problem+json`) with a generic title and a trace id. In development the detail includes the exception.
  * MVC will redirect to secure error or forbidden pages in non-development.
* Routing for SPA and Web API. Unmatched `/api` requests return 404 instead of the SPA.
* A health check endpoint (`MapHealthEndpoint`) that returns 200 or 503 with each check's status in a JSON body. Pass `includeDetails: true`, or a `Func<HttpContext, bool>` to decide per request (for example only for authenticated users), to also include each check's description and exception message for trusted monitors.
* HttpResponder for converting Domain Event responses to IActionResult with appropriate status codes.
