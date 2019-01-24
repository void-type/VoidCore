# VoidCore.AspNet

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg?style=flat-square)](https://www.nuget.org/packages/VoidCore.AspNet/)

## Installation

```powerShell
dotnet add package VoidCore.AspNet
```

## Features

VoidCore.AspNet includes helpers for configuring an ASP.NET Core web application:

* Serilog multi-platform file logging.
* Active Directory group authorization via Windows authentication.
* HTTPS with redirection and HSTS headers.
* Antiforgery for SPAs.
* Exception handling (SPA and MVC) with logging.
  * API endpoints return a JSON {message: ""} object.
  * MVC will redirect to secure error or forbidden pages in non-development.
* Routing for SPA and Web API.
* Data repositories implementation for EF Core.
* HttpResponder for converting Domain Event Responses to IActionResult.
