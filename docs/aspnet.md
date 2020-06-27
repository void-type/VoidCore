# VoidCore.AspNet

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.AspNet.svg)](https://www.nuget.org/packages/VoidCore.AspNet/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.AspNet.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.AspNet)

## Installation

```powerShell
dotnet add package VoidCore.AspNet
```

## Features

VoidCore.AspNet is an ASP.NET implementation of VoidCore.Model. It includes helpers for configuring an ASP.NET Core web application:

* Active Directory group authorization via Windows authentication.
* HTTPS with redirection and HSTS headers.
* Antiforgery for SPAs.
* Exception handling (SPA and MVC) with logging.
  * API endpoints return a JSON {message: ""} object for the client.
  * MVC will redirect to secure error or forbidden pages in non-development.
* Routing for SPA and Web API.
* HttpResponder for converting Domain Event responses to IActionResult with appropriate status codes.
