# CLAUDE.md

Guidance for working in the **ai-engine** repository.

## Overview

`ai-engine` is a .NET 10 (`net10.0`) ASP.NET Core solution laid out in Clean
Architecture style. It exposes minimal-API endpoints for chatting with an LLM
and for provisioning a Qdrant vector store. The main flow: an HTTP request is
turned into a CQRS command, dispatched through a hand-rolled mediator
(validation + diagnostics pipeline behaviors), and handled by talking to an AI
provider (OpenRouter) or the vector database (Qdrant).

Cross-cutting concerns already in place: Serilog structured logging with a
single summary line per request, a `Result`/`ProblemDetails` (RFC 9457) error
model, FluentValidation in the pipeline, Swagger/OpenAPI, and HTTP resilience
for outbound AI calls.

## Solution layout

The solution file is [src/Engine.slnx](src/Engine.slnx) (the newer XML `.slnx`
format). All projects live under [src/](src/):

| Project | Solution folder | SDK | Role |
|---|---|---|---|
| [Engine.Api](src/Engine.Api/) | `/Web/` | `Microsoft.NET.Sdk.Web` | HTTP entry point, minimal-API endpoints, Serilog, Swagger/OpenAPI, composition root |
| [Engine.Application](src/Engine.Application/) | `/Application/` | `Microsoft.NET.Sdk` | CQRS features, mediator, pipeline behaviors, validators, AI/vector interfaces & models |
| [Engine.Domain](src/Engine.Domain/) | `/Domain/` | `Microsoft.NET.Sdk` | `Result` pattern and error types (no dependencies) |
| [Engine.Infrastructure](src/Engine.Infrastructure/) | `/Infrastructure/` | `Microsoft.NET.Sdk` | AI provider adapters (OpenRouter, Test) and their factory |
| [Engine.VectorDBContext](src/Engine.VectorDBContext/) | `/Infrastructure/` | `Microsoft.NET.Sdk` | Qdrant client, vector storage service, embedding client |

### Dependency direction (Clean Architecture) — now wired up

```
Engine.Api ──► Engine.Application ──► Engine.Domain
Engine.Infrastructure   ──► Engine.Application
Engine.VectorDBContext  ──► Engine.Application
Engine.Api ──► Engine.Domain / Engine.Infrastructure / Engine.VectorDBContext   (composition root / DI)
```

`Engine.Domain` references no other layer. The AI and vector-storage
**interfaces** (`IAiClient`, `IAiClientFactory`, `IVectorStorageService`,
`IEmbeddingClient`) live in `Engine.Application/Interfaces`; the concrete
implementations live in the two infrastructure projects.

## Key conventions

- **CQRS features** live in `Engine.Application/Features/<Area>/<Name>.cs` as a
  single static class nesting `Command`/`Query`, `Handler`, and `Validator`
  (see [SendMessage.cs](src/Engine.Application/Features/Chats/SendMessage.cs)).
- **Mediator** is hand-rolled ([Messaging/](src/Engine.Application/Messaging/)):
  inject `ISender` and call `Send`. Handlers and validators are auto-registered
  by `AddEngineApplication`. Pipeline order is `DiagnosticsBehavior` (outer) →
  `ValidationBehavior` — input validation happens here, not in the Web layer.
- **Endpoints** implement `IEndpoint`
  ([EndpointsSetting/IEndpoint.cs](src/Engine.Api/EndpointsSetting/IEndpoint.cs)),
  are discovered via `AddEndpoints`, and mapped by `MapEndpoints`. Route
  prefixes are centralized in
  [EngineRoutes](src/Engine.Api/Constants/EngineRoutes.cs) (`/api/...`).
- **Results & errors**: handlers return `Result`/`Result<T>` from
  `Engine.Domain.Common`; endpoints translate failures to `ProblemDetails` via
  [ResultExtensions.ToProblem](src/Engine.Api/Extensions/ResultExtensions.cs).
- **AI providers**: `IAiClientFactory.Create(AiProvider)` resolves a
  keyed `IAiClient` adapter. Adding a provider = new adapter + one keyed
  registration block in
  [InfrastructureServiceCollectionExtensions](src/Engine.Infrastructure/Extensions/InfrastructureServiceCollectionExtensions.cs).
- DI wiring for the whole app is composed in
  [ApplicationExtensions.TuneEngineApplication](src/Engine.Api/Extensions/ApplicationExtensions.cs).

## Build settings

Enforced solution-wide via [src/Directory.Build.props](src/Directory.Build.props):
`net10.0`, `Nullable` + `ImplicitUsings` enabled, `AnalysisMode=All`,
**`TreatWarningsAsErrors=true`**, `EnforceCodeStyleInBuild`,
`GenerateDocumentationFile`, and **StyleCop.Analyzers** (1.1.118). XML doc
comments are required on public members. NuGet uses **central package
management** — declare versions in
[src/Directory.Packages.props](src/Directory.Packages.props), reference without
versions in each `.csproj`.

## Build & run

Run all commands from the [src/](src/) directory (where `Engine.slnx` lives).

```powershell
dotnet build Engine.slnx
dotnet run --project Engine.Api          # http profile
dotnet run --project Engine.Api --launch-profile https
```

- HTTP: `http://localhost:5153`
- HTTPS: `https://localhost:7029`

Swagger UI and OpenAPI (`/openapi`) are mapped only in the Development
environment.

### External dependencies

- **Qdrant** must be reachable at the `ConnectionStrings:Qdrant` gRPC endpoint
  (`http://localhost:6334` in
  [appsettings.Development.json](src/Engine.Api/appsettings.Development.json)) —
  DI startup throws if it is missing. `POST /api/development/create-collections`
  provisions the initial collections.
- **OpenRouter** is configured under the `Ai:OpenRouter` section
  ([appsettings.json](src/Engine.Api/appsettings.json)); `POST /api/chats/send`
  drives a completion through it.

## Notes

- No test projects exist yet. When adding tests, prefer one test project per
  layer (e.g. `Engine.Domain.Tests`) and add it to `Engine.slnx`.
- `StubEmbeddingClient` is a placeholder embedding implementation — replace it
  with a real embedding provider before relying on vector search.
- **Do not commit secrets.** `appsettings.json` currently contains a real-looking
  OpenRouter `ApiKey`; move provider keys to user-secrets / environment
  variables and rotate any key that has been committed.
