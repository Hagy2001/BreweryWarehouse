---
name: logging-skill
description: Conventions for adding or modifying structured logging in BreweryWarehouse.Web using Serilog
---

## Scope
Adding logging to new controllers, modifying log levels, updating sink configuration, or diagnosing why events are or aren't appearing in logs.

## Library
**Serilog only.** Do not introduce a second logging library (NLog, log4net, Microsoft.Extensions.Logging file provider, etc.).
Packages already in the project:
- `Serilog.AspNetCore` — hosting integration, Console sink, config binding
- `Serilog.Sinks.File` — rolling file sink

## Bootstrap
`Program.cs` uses the two-stage Serilog init pattern — a bootstrap logger is created before `WebApplication.CreateBuilder` so startup failures are captured. The try/catch/finally wraps the entire application body and calls `Log.CloseAndFlush()` on exit.

## File path resolution (Azure vs local)
The file sink path is resolved in code in `Program.cs` — **do not hardcode a single path**:

```csharp
var logPath = Directory.Exists("/home")
    ? "/home/LogFiles/Application/log-.txt"   // Azure App Service (Linux) — persistent storage
    : "logs/log-.txt";                         // local dev — relative to working dir
```

`/home` is the mount point for persistent storage on Azure App Service Linux; any other path is ephemeral and lost on redeploy. Locally `/home` does not typically exist so the relative `logs/` path is used.

The file sink is added in code (not in `appsettings.json`) so the path logic applies at runtime:
```csharp
builder.Host.UseSerilog((ctx, svc, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .ReadFrom.Services(svc)
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14));
```

## Log-level table

| Source | Level | Reason |
|---|---|---|
| `BreweryWarehouse.*` | Information | App's own code — keep all signal |
| `Microsoft.AspNetCore` | Warning | Suppress routine request noise |
| `Microsoft.AspNetCore.Authorization` | **Information** | Surfaces access-denied events cheaply (e.g. Google OAuth users with no role hitting Authorize) |
| `Microsoft.EntityFrameworkCore` | Warning | Suppress per-query SQL noise |

These are set in `appsettings.json` under `Serilog.MinimumLevel.Override`. The Console sink is also configured there; the File sink path is code-only (see above).

Rolling interval: Day. Retained file count: 14 (keeps storage use bounded on Azure F1 tier's 1 GB cap).

## Controller logging conventions
**Every new entity controller must follow this pattern:**

- Inject `ILogger<TController>` via constructor DI (not `ILoggerFactory`, not static).
- **Create/Edit/Delete POST** actions only — do **not** log Index, Details, or Search GETs (noise for a low-traffic app).
- On **validation failure** (ModelState.IsValid == false): `LogWarning` with entity type, identifying field, and user.
- On **success**: `LogInformation` after the repository call succeeds, with entity Id, key display field, and user.
- Use **structured properties** in message templates (`{Id}`, `{User}`) — never string-interpolate values directly into the template.

Example pattern:
```csharp
// validation failure
_logger.LogWarning("BeerStyle creation failed validation for {Name} by {User}", model.Name, User.Identity?.Name);

// success
_logger.LogInformation("BeerStyle {Id} '{Name}' created by {User}", beerStyle.Id, beerStyle.Name, User.Identity?.Name);
```

## Never log secrets
Do not log any of the following, even accidentally via object serialization:
- Connection strings
- Passwords or password hashes
- Google OAuth ClientSecret or access/refresh tokens
- Full Identity tokens or session cookies
- Complete model objects that contain any of the above

When logging a model, log only the non-sensitive identifying fields explicitly.
