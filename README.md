# Whipstaff

Whipstaff is a multi-project .NET/C# collection of libraries, UI integrations, developer tools, and example apps. It provides reusable building blocks for web, desktop, and cross-platform applications (ASP.NET Core, Blazor, WPF, MAUI, WinUI, Avalonia), integrations with common libraries (Entity Framework, MediatR, Syncfusion, Playwright, Markdig, Mermaid, OpenXml), and several developer tools (dotnet tools, diagram & markdown generators).

## Table of contents
- About
- Build & test
- Repository layout — notable files
- Projects
- Examples & samples
- Testing & benchmarks
- Contributing
- License
- Contact

## About
This repository is organized as a multi-project .NET solution (Whipstaff.sln) composed of many small focused projects. Most projects are libraries or integrations intended to be used independently or composed together. The design favors small, single-responsibility packages and includes multiple UI adapter libraries and provider/adapters for persistence and tooling.

## Tenant lookup with Finbuckle

`Whipstaff.AspNetCore.MultiTenancy` provides a Finbuckle EF Core store and lookup table for resolving tenants by an authenticated Microsoft Entra tenant ID (`tid`), an authenticated email claim, or a hostname prefix. Choose one `TenantLookupSource` per registration; there is no automatic fallback between sources. For example, with an EF Core SQL Server provider installed:

```csharp
using Finbuckle.MultiTenant.AspNetCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Whipstaff.AspNetCore.MultiTenancy;

builder.Services.AddLookupTenantResolution(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("TenantLookup")),
    TenantLookupSource.HostPrefix,
    hostDomain: "example.com");

// Configure trusted proxies and forwarded headers before this point when applicable.
app.UseForwardedHeaders();
app.UseMultiTenant();
app.UseAuthentication();
app.UseAuthorization();
```

The independent `TenantLookupDbContext` has a Finbuckle `TenantInfo` table and a `Lookups` table with a unique `(Source, Value)` key and a foreign key to the tenant's stable ID. Create and apply EF Core migrations for your chosen provider. Add each tenant's canonical `Id` and `Identifier`, then add lookup rows using `TenantLookup.Normalize(source, value)` for `Value`; GUIDs use canonical format, email is uppercased, and host prefixes are lowercased. Multiple keys can point to the same tenant. Provision lookup rows only from trusted administrative processes and handle duplicate-key errors explicitly.

For example, provision a tenant and a host lookup in a scoped administrative operation:

```csharp
using Finbuckle.MultiTenant.Abstractions;

lookupDb.TenantInfo.Add(new TenantInfo { Id = "tenant-1", Identifier = "tenant-1" });
lookupDb.Lookups.Add(new TenantLookup
{
    TenantId = "tenant-1",
    Source = TenantLookupSource.HostPrefix,
    Value = TenantLookup.Normalize(TenantLookupSource.HostPrefix, "customer"),
});
await lookupDb.SaveChangesAsync();
```

For Entra ID or email resolution, configure an authentication scheme that validates the token issuer, audience and signature, and put `UseAuthentication()` **before** `UseMultiTenant()` so authenticated claims are available. The resolver accepts `tid` or the mapped Microsoft tenant ID claim, and `email` or the mapped email claim; missing, conflicting, or unauthenticated claims do not resolve. This ordering requires authentication that does not itself depend on the tenant being resolved. Email claims may be absent, mutable or not verified; they must not be used as proof of identity or authorization. Prefer stable Entra tenant IDs when possible.

For hostname resolution, the host must be exactly one label under the configured parent domain. The resolver reads `Request.Host`, never raw `X-Forwarded-Host`. Behind a proxy or WAF, configure ASP.NET Core forwarded headers with explicit known proxy addresses or networks, an appropriate forward limit and allowed hosts, and run it before tenant resolution. Ensure the edge overwrites untrusted forwarded headers and direct access to the application is restricted. Also configure host filtering at the edge/application. A hostname selects a tenant; it does not authorize a user for that tenant. Applications must reject unresolved tenants where required and independently enforce tenant membership and authorization for every request.

See [Finbuckle stores](https://www.finbuckle.com/Docs/v10.2.0/Stores) and [ASP.NET Core proxy configuration](https://learn.microsoft.com/aspnet/core/host-and-deploy/proxy-load-balancer) for provider and proxy setup.

## Build & test
To build everything from the solution root:
```bash
dotnet build src/Whipstaff.sln
```

To run unit and integration tests:
```bash
dotnet test src/Whipstaff.UnitTests
dotnet test src/Whipstaff.IntegrationTests
```

To run or build a sample app (example):
```bash
cd src/Whipstaff.Example.WebApiApp
dotnet run
```

For the dotnet tools included in the repo (MarkdownGen, EF diagram generator), run them via `dotnet run` from the tool project folder during development or install them when published:
```bash
# Example local invocation pattern
dotnet run --project src/Whipstaff.CommandLine.MarkdownGen.DotNetTool
```

## Repository layout — notable files
- src/Whipstaff.sln — main solution file that ties projects together  
- src/Directory.build.props, src/Directory.build.targets — centralized MSBuild configuration used by projects  
- src/stylecop.json, src/analyzers.ruleset — shared code-style and analyzer configurations  
- src/_shared — shared source/items used across multiple projects (MSBuild SharedItems)  
- src/*.csproj — project-level metadata and package references

## Projects
Below is a table summarizing the projects in src/, their likely purpose, primary technology, and type. Use the links to inspect each project for exact metadata and usage.

| Project | Purpose | Primary tech | Type | Link |
|---|---|---:|---|---|
| Whipstaff.Avalonia | Avalonia UI components for cross-platform desktop | C# / Avalonia | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Avalonia |
| Whipstaff.AspNetCore | ASP.NET Core middleware and helpers | C# / ASP.NET Core | Library / Middleware | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.AspNetCore |
| Whipstaff.AspNetCore.StaticFiles | Static/embedded static-file helpers for ASP.NET Core | C# / ASP.NET Core | Middleware | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.AspNetCore.StaticFiles |
| Whipstaff.Benchmarks | BenchmarkDotNet benchmarks for performance testing | C# / BenchmarkDotNet | Benchmarks | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Benchmarks |
| Whipstaff.Blazor | Blazor UI utilities and shared components | C# / Blazor | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor |
| Whipstaff.Blazor.MudBlazor | MudBlazor integration/wrappers for Blazor | C# / MudBlazor | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.MudBlazor |
| Whipstaff.Blazor.Syncfusion | ReactiveUI Vetuviem Binding Models for Syncfusion Blazor components | C# / Syncfusion / Blazor | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion |
| Whipstaff.Blazor.Syncfusion.BarcodeGenerator | ReactiveUI Vetuviem Binding Models for the Syncfusion BarcodeGenerator component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.BarcodeGenerator |
| Whipstaff.Blazor.Syncfusion.Buttons | ReactiveUI Vetuviem Binding Models for the Syncfusion Buttons component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Buttons |
| Whipstaff.Blazor.Syncfusion.Calendars | ReactiveUI Vetuviem Binding Models for the Syncfusion Calendars component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Calendars |
| Whipstaff.Blazor.Syncfusion.Charts | ReactiveUI Vetuviem Binding Models for the Syncfusion Charts component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Charts |
| Whipstaff.Blazor.Syncfusion.Core | ReactiveUI Vetuviem Binding Models and core helpers for Syncfusion integrations | C# / Syncfusion | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Core |
| Whipstaff.Blazor.Syncfusion.Data | ReactiveUI Vetuviem Binding Models for Syncfusion data components | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Data |
| Whipstaff.Blazor.Syncfusion.DataVizCommon | ReactiveUI Vetuviem Binding Models and shared data-viz helpers for Syncfusion | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.DataVizCommon |
| Whipstaff.Blazor.Syncfusion.Diagram | ReactiveUI Vetuviem Binding Models for the Syncfusion Diagram component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Diagram |
| Whipstaff.Blazor.Syncfusion.DropDowns | ReactiveUI Vetuviem Binding Models for the Syncfusion DropDowns component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.DropDowns |
| Whipstaff.Blazor.Syncfusion.Inputs | ReactiveUI Vetuviem Binding Models for the Syncfusion Inputs component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Inputs |
| Whipstaff.Blazor.Syncfusion.Lists | ReactiveUI Vetuviem Binding Models for the Syncfusion Lists component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Lists |
| Whipstaff.Blazor.Syncfusion.Navigations | ReactiveUI Vetuviem Binding Models for the Syncfusion Navigations component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Navigations |
| Whipstaff.Blazor.Syncfusion.Notifications | ReactiveUI Vetuviem Binding Models for the Syncfusion Notifications component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Notifications |
| Whipstaff.Blazor.Syncfusion.Popups | ReactiveUI Vetuviem Binding Models for the Syncfusion Popups component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Popups |
| Whipstaff.Blazor.Syncfusion.Schedule | ReactiveUI Vetuviem Binding Models for the Syncfusion Schedule component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Schedule |
| Whipstaff.Blazor.Syncfusion.Spinner | ReactiveUI Vetuviem Binding Models for the Syncfusion Spinner component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.Spinner |
| Whipstaff.Blazor.Syncfusion.SplitButtons | ReactiveUI Vetuviem Binding Models for the Syncfusion SplitButtons component | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Blazor.Syncfusion.SplitButtons |
| Whipstaff.CommandLine | Command-line utilities and helpers | C# / .NET | CLI / Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.CommandLine |
| Whipstaff.CommandLine.MarkdownGen.DotNetTool | dotnet tool to generate Markdown documentation | C# / dotnet tool | Tool | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.CommandLine.MarkdownGen.DotNetTool |
| Whipstaff.Core | Core utilities and primitives shared across packages | C# / .NET | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Core |
| Whipstaff.Couchbase | Couchbase provider utilities | C# / Couchbase | Provider | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Couchbase |
| Whipstaff.CsvHelper | CSV parsing/writing utilities | C# / CsvHelper | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.CsvHelper |
| Whipstaff.EntityFramework | Entity Framework helper utilities | C# / EF Core | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.EntityFramework |
| Whipstaff.EntityFramework.Diagram.DotNetTool | Tool for generating EF diagrams/schema visualizations | C# / dotnet tool | Tool | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.EntityFramework.Diagram.DotNetTool |
| Whipstaff.Entityframework.Relational | Relational EF helpers and adapters | C# / EF Core | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Entityframework.Relational |
| Whipstaff.Example.AspireAppHost | Example Aspire host demonstrating hosting patterns | C# / Aspire | Example app | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Example.AspireAppHost |
| Whipstaff.Example.AspireServiceDefaults | Default services for Aspire examples | C# / Aspire | Example helper | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Example.AspireServiceDefaults |
| Whipstaff.Example.WebApiApp | Example ASP.NET Core Web API demonstrating usage | C# / ASP.NET Core | Example app | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Example.WebApiApp |
| Whipstaff.Example.WebMvcApp | Example ASP.NET Core MVC app | C# / ASP.NET Core MVC | Example app | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Example.WebMvcApp |
| Whipstaff.Example.WpfApp | Example WPF desktop app | C# / WPF | Example app | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Example.WpfApp |
| Whipstaff.Healthchecks.EntityFramework | EF-based health checks for ASP.NET Core | C# / HealthChecks / EF Core | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Healthchecks.EntityFramework |
| Whipstaff.IntegrationTests | Integration test project | C# / test frameworks | Tests | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.IntegrationTests |
| Whipstaff.Markdig | Markdown processing utilities (Markdig) | C# / Markdig | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Markdig |
| Whipstaff.Maui | .NET MAUI components and helpers | C# / .NET MAUI | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Maui |
| Whipstaff.Maui.AspNetCore.WebView | WebView integration to host ASP.NET Core in MAUI | C# / MAUI / ASP.NET Core | Integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Maui.AspNetCore.WebView |
| Whipstaff.Maui.Syncfusion.Toolkit | ReactiveUI Vetuviem Binding Models for Syncfusion toolkit (MAUI) | C# / Syncfusion / MAUI | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Maui.Syncfusion.Toolkit |
| Whipstaff.MediatR | MediatR helpers and patterns | C# / MediatR | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.MediatR |
| Whipstaff.MediatR.EntityFrameworkCore | MediatR + EF Core integration helpers | C# / MediatR / EF Core | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.MediatR.EntityFrameworkCore |
| Whipstaff.MediatR.Foundatio | MediatR + Foundatio integration helpers | C# / MediatR / Foundatio | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.MediatR.Foundatio |
| Whipstaff.Mermaid | Mermaid diagram generation utilities | C# | Tool/Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Mermaid |
| Whipstaff.MsSql | MSSQL provider/adapters and helpers | C# / MSSQL | Provider | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.MsSql |
| Whipstaff.Nuget | Utilities for NuGet packaging/publishing | C# / NuGet | Tool/Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Nuget |
| Whipstaff.OpenXml | Utilities for working with OpenXML documents | C# / OpenXML SDK | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.OpenXml |
| Whipstaff.Oracle | Oracle provider/adapters and helpers | C# / Oracle | Provider | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Oracle |
| Whipstaff.Playwright | Playwright helpers for browser automation & E2E | C# / Playwright | Testing helper | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Playwright |
| Whipstaff.ReactiveUI | ReactiveUI helpers and view-model utilities | C# / ReactiveUI | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.ReactiveUI |
| Whipstaff.Rx | Reactive Extensions (Rx) helpers | C# / Rx.NET | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Rx |
| Whipstaff.Statiq | Statiq modules/integrations for static site generation | C# / Statiq | Tool/Plugin | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Statiq |
| Whipstaff.Syncfusion.Pdf | Helpers for generating PDFs (Syncfusion) | C# / Syncfusion | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Syncfusion.Pdf |
| Whipstaff.TestableIO.System.IO.Abstractions.TestingHelpers | Testable file I/O helpers (System.IO.Abstractions) | C# / System.IO.Abstractions | Testing helper | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.TestableIO.System.IO.Abstractions.TestingHelpers |
| Whipstaff.Testing | Testing utilities and helpers | C# / test frameworks | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Testing |
| Whipstaff.UnitTests | Unit test project | C# / test frameworks | Tests | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.UnitTests |
| Whipstaff.WinUI | WinUI helpers and components | C# / WinUI | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.WinUI |
| Whipstaff.Windows | Windows-specific helpers/integrations | C# / Windows | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Windows |
| Whipstaff.Wpf | WPF helpers and components | C# / WPF | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf |
| Whipstaff.Wpf.AvalonEdit | AvalonEdit integration for WPF | C# / AvalonEdit | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.AvalonEdit |
| Whipstaff.Wpf.AspNetCore.WebView | WebView integration to host ASP.NET Core in WPF | C# / WPF / ASP.NET Core | Integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.AspNetCore.WebView |
| Whipstaff.Wpf.CefSharp | CefSharp (embedded browser) integration for WPF | C# / CefSharp | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.CefSharp |
| Whipstaff.Wpf.Mahapps | MahApps theming/controls integrations for WPF | C# / MahApps | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.Mahapps |
| Whipstaff.Wpf.MaterialDesign | MaterialDesign integration for WPF | C# / MaterialDesign | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.MaterialDesign |
| Whipstaff.Wpf.Syncfusion.SfDiagram | ReactiveUI Vetuviem Binding Models for the Syncfusion SfDiagram component (WPF) | C# / Syncfusion | UI integration | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Wpf.Syncfusion.SfDiagram |
| Whipstaff.Runtime | Runtime and hosting helpers | C# / .NET | Library | https://github.com/dpvreony/whipstaff/tree/main/src/Whipstaff.Runtime |

## Examples & sample apps
- Whipstaff.Example.WebApiApp — example ASP.NET Core Web API demonstrating usage with Whipstaff libraries.  
- Whipstaff.Example.WebMvcApp — MVC sample app.  
- Whipstaff.Example.WpfApp — WPF sample application demonstrating desktop scenarios.  
- Whipstaff.Example.AspireAppHost, Whipstaff.Example.AspireServiceDefaults — Aspire hosting examples.

## Testing & benchmarks
- Unit tests: src/Whipstaff.UnitTests  
- Integration tests: src/Whipstaff.IntegrationTests  
- Benchmarks: src/Whipstaff.Benchmarks

Run with:
```bash
dotnet test src/Whipstaff.UnitTests
dotnet test src/Whipstaff.IntegrationTests
dotnet run --project src/Whipstaff.Benchmarks
```

## Contributing
- Open an issue to propose major changes.  
- Fork the repo and create a topic branch for changes.  
- Add tests for new features and run linters/formatters.  
- Follow the repository's StyleCop/analyzer rules (see src/stylecop.json and src/analyzers.ruleset).  

## License
This project is licensed under the MIT License — see the LICENSE file for details.
