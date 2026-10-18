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
  * API endpoints return a JSON {message: ""} object for the client.
  * MVC will redirect to secure error or forbidden pages in non-development.
* Routing for SPA and Web API.
* HttpResponder for converting Domain Event responses to IActionResult with appropriate status codes.
