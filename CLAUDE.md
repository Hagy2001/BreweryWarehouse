## Project: BreweryWarehouse
A craft brewery warehouse management system built with ASP.NET Core MVC / C# .NET 8.

## Solution Structure
- `BreweryWarehouse.Model` — class library, all domain classes
- `BreweryWarehouse.Console` — LINQ queries and seed data (Lab 1)
- `BreweryWarehouse.Web` — ASP.NET Core MVC web app (active from Lab 2)
- `BreweryWarehouse.Tests` — xUnit integration tests (fast, in-memory, no browser)
- `BreweryWarehouse.E2ETests` — Playwright E2E tests (slow, real browser, app must be running)
- `docs/semantic-model.md` — AI context document for the database/domain model
- `docs/sitemap.md` — AI routing sitemap for app URLs
- `.github/skills/ef-skill.md` — EF Core workflow skill for BreweryWarehouse
- `.github/skills/list-page-skill.md` — list page workflow skill for BreweryWarehouse.Web

## Conventions
- Namespace: BreweryWarehouse.Model for all model classes
- No public fields, use public auto-properties only
- Initialize all List<> properties in constructors
- All classes must be public

## Web Conventions (BreweryWarehouse.Web)
- Namespace: BreweryWarehouse.Web for controllers and ViewModels
- Controllers go in Controllers/ and must end with "Controller" suffix
- Views go in Views/{ControllerName}/{ActionName}.cshtml
- Mock repositories go in Repositories/ and end with "MockRepository"
- Register mock repositories as AddSingleton in Program.cs
- Use Tag Helpers (<a asp-controller asp-action>) for all internal links
- No logic in views except simple if/foreach and TagHelpers

## Current Web Artifacts
- BeerStyleRepository: Repositories/BeerStyleRepository.cs with EF Core GetAll()/GetById() including Cans and Kegs plus Add/Update/SoftDelete
- CanRepository: Repositories/CanRepository.cs with EF Core GetAll()/GetById() including BeerStyle and StockEntries plus Add/Update/SoftDelete
- KegRepository: Repositories/KegRepository.cs with EF Core GetAll()/GetById() including BeerStyle and StockEntries plus Add/Update/SoftDelete
- WarehouseLocationRepository: Repositories/WarehouseLocationRepository.cs with EF Core GetAll()/GetById() including StockEntries and Container BeerStyle plus Add/Update/Delete
- StockEntryRepository: Repositories/StockEntryRepository.cs with EF Core GetAll()/GetById() including Container BeerStyle and Location plus Add/Update/Delete
- EmployeeRepository: Repositories/EmployeeRepository.cs with EF Core GetAll() and GetById(int id) plus Add/Update/Delete
- Controllers: all controllers now inject EF repositories instead of mock repositories
- BeerStyleMockRepository: Repositories/BeerStyleMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- BeerStyleController: Controllers/BeerStyleController.cs with Index(), Details(int id), Create, Edit, Delete, Search(string q)
- BeerStyle Views: Views/BeerStyle/Index.cshtml, Views/BeerStyle/Details.cshtml, Views/BeerStyle/Create.cshtml, Views/BeerStyle/Edit.cshtml, Views/BeerStyle/_CreateOrEdit.cshtml
- CanMockRepository: Repositories/CanMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- CanController: Controllers/CanController.cs with [Authorize], Index(), Details(int id), Create, Edit, Delete, Search(string q), PopulateBeerStyles() — injects BeerStyleRepository, ViewBag.BeerStyles SelectList used by Can/_CreateOrEdit
- Can Views: Views/Can/Index.cshtml, Views/Can/Details.cshtml, Views/Can/Create.cshtml, Views/Can/Edit.cshtml, Views/Can/_CreateOrEdit.cshtml
- KegMockRepository: Repositories/KegMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- KegController: Controllers/KegController.cs with [Authorize], Index(), Details(int id), Create, Edit, Delete, Search(string q), PopulateBeerStyles() — injects BeerStyleRepository, ViewBag.BeerStyles SelectList used by Keg/_CreateOrEdit
- Keg Views: Views/Keg/Index.cshtml, Views/Keg/Details.cshtml, Views/Keg/Create.cshtml, Views/Keg/Edit.cshtml, Views/Keg/_CreateOrEdit.cshtml
- WarehouseLocationMockRepository: Repositories/WarehouseLocationMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- WarehouseLocationController: Controllers/WarehouseLocationController.cs with [Authorize], Index(), Details(int id), Create, Edit, Delete, Search(string q)
- WarehouseLocation Views: Views/WarehouseLocation/Index.cshtml, Views/WarehouseLocation/Details.cshtml, Views/WarehouseLocation/Create.cshtml, Views/WarehouseLocation/Edit.cshtml, Views/WarehouseLocation/_CreateOrEdit.cshtml
- StockEntryMockRepository: Repositories/StockEntryMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- StockEntryController: Controllers/StockEntryController.cs with [Authorize], Index(), Details(int id), Create, Edit, Delete, Search(string q) plus PopulateDropdowns() for Containers/Locations
- StockEntry Views: Views/StockEntry/Index.cshtml, Views/StockEntry/Details.cshtml, Views/StockEntry/Create.cshtml, Views/StockEntry/Edit.cshtml, Views/StockEntry/_CreateOrEdit.cshtml
- EmployeeMockRepository: Repositories/EmployeeMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- EmployeeController: Controllers/EmployeeController.cs with [Authorize], Index(), Details(int id) (async, loads LinkedUser via UserManager), Create, Edit (GET populates ViewBag.AppUsers SelectList, POST saves AppUserId), Delete, Search(string q) — injects UserManager&lt;AppUser&gt;
- Employee Views: Views/Employee/Index.cshtml, Views/Employee/Details.cshtml, Views/Employee/Create.cshtml, Views/Employee/Edit.cshtml, Views/Employee/_CreateOrEdit.cshtml (bw-badge--active/inactive status badges in site.css)
- HomeController: Controllers/HomeController.cs with [Authorize], BeerStyleRepository, CanRepository, KegRepository, WarehouseLocationRepository, StockEntryRepository injection, dashboard Index() projection (includes TotalStockUnits), Privacy(), Error()
- Home Dashboard View: Views/Home/Index.cshtml with KPI cards, expiring combined table, and stock-by-location utilization bars
- Shared Layout: Views/Shared/_Layout.cshtml uses brewery sidebar navigation and shared site.css theme
- DatePicker Partial: Views/Shared/_DatePicker.cshtml for custom date inputs
- BeerStyle Autocomplete Partial: Views/Shared/_BeerStyleAutocomplete.cshtml for AJAX beer style lookup inputs
- AppUser: Models/AppUser.cs — extends IdentityUser with OIB (11-digit) and JMBG (13-digit) string properties
- IdentitySeeder: Services/IdentitySeeder.cs — seeds Admin and WarehouseManager roles; seeds default admin@brewery.com / Admin123! with Admin role on startup
- Identity Register: Areas/Identity/Pages/Account/Register.cshtml + .cs — custom register page with OIB and JMBG fields, no shared layout
- Google OAuth: Program.cs configured with AddGoogle reading Authentication:Google:ClientId and ClientSecret from user secrets
- Login View: Areas/Identity/Pages/Account/Login.cshtml — brewery-styled login page with local form and Google login button
- ExternalLogin View: Areas/Identity/Pages/Account/ExternalLogin.cshtml + .cs — handles Google OAuth callback, collects OIB and JMBG on first login, creates AppUser and signs in
- Logout Page: Areas/Identity/Pages/Account/Logout.cshtml.cs — signs out and redirects to /
- LoginPartial: Views/Shared/_LoginPartial.cshtml — Identity-aware login/logout partial injected into sidebar
- Load animation overlay: _Layout.cshtml overlay injection, wwwroot/css/load-animation.css keyframes, wwwroot/js/load-animation.js sequencing — amber liquid fills viewport bottom-to-top (2500ms cubic-bezier), SMIL wave/foam surface morphing (3s loop, opposite-phase secondary), logo revealed bottom-to-top via SVG mask (defs must be colocated in same SVG as masked element — Chrome requirement), 12 bubbles single-run with slow fade-out (1 forwards), dismiss curtain at 3050ms, skipped on prefers-reduced-motion. See .github/skills/load-animation-skill.md and .github/agents/animation-agent.agent.md for full spec
- Brand assets: wwwroot/images/logo.svg (The Garden Brewery SVG, fill-inheritable)
- Swagger: available at /swagger in Development; registered via AddEndpointsApiExplorer + AddSwaggerGen in Program.cs
- BeerStyleApiController: Controllers/Api/BeerStyleApiController.cs — [ApiController] [Route("api/beer-styles")] full CRUD, soft delete, GET AllowAnonymous, POST/PUT Admin+WarehouseManager, DELETE Admin
- CanApiController: Controllers/Api/CanApiController.cs — [Route("api/cans")] full CRUD, soft delete, same auth rules
- KegApiController: Controllers/Api/KegApiController.cs — [Route("api/kegs")] full CRUD, soft delete, same auth rules
- WarehouseLocationApiController: Controllers/Api/WarehouseLocationApiController.cs — [Route("api/locations")] full CRUD, hard delete, same auth rules
- StockEntryApiController: Controllers/Api/StockEntryApiController.cs — [Route("api/stock-entries")] full CRUD, hard delete, same auth rules
- EmployeeApiController: Controllers/Api/EmployeeApiController.cs — [Route("api/employees")] full CRUD, hard delete, same auth rules
- DtoExtensions: Models/Dtos/DtoExtensions.cs — ToDto() and ToSummaryDto() extension methods for all 6 entities
- DTOs: Models/Dtos/ — BeerStyleDto, CanDto, KegDto, WarehouseLocationDto, StockEntryDto, EmployeeDto, ContainerSummaryDto (all response DTOs) plus BeerStyleRequestDto, CanRequestDto, KegRequestDto, WarehouseLocationRequestDto, StockEntryRequestDto, EmployeeRequestDto (all request DTOs with DataAnnotations)
- Attachment entity: BreweryWarehouse.Model/Attachment.cs — Id, StockEntryId, StockEntry (nav), FileName, FilePath, ContentType, FileSize, CreatedAt; Cascade delete on StockEntry removal
- StockEntry Attachments: StockEntryController has UploadAttachment, GetAttachments, DeleteAttachment actions; files saved to wwwroot/uploads/stock-entries/{id}/; metadata in Attachments table
- AttachmentList partial: Views/StockEntry/_AttachmentList.cshtml — table of attachments with download links and delete buttons
- Dropzone: wwwroot/lib/dropzone v5.9.3 — used on StockEntry Edit view for async optional file upload
- StockEntry Edit view: includes optional Dropzone upload section and AJAX attachment list loaded on page load and after each upload
- StockEntry Details view: shows read-only attachment list via ViewBag.Attachments populated in Details() action
- Integration Tests: BreweryWarehouse.Tests project — xUnit tests covering full CRUD for all 6 API endpoints (BeerStyleApi, CanApi, KegApi, WarehouseLocationApi, StockEntryApi, EmployeeApi); uses WebApplicationFactory with InMemory EF database; BreweryWarehouseWebApplicationFactory swaps SQL Server for InMemory and stubs Google OAuth config; ApiTestBase provides seed helpers and authenticated client creation
- BeerStyleApiTests: xUnit tests with GetAll, GetById (found/not found), GetAll_WithQuery search, Post (unauth/201/400), Put (200/404), Delete (204/404/403)
- CanApiTests: xUnit tests with GetAll, GetById (found/not found), GetAll_WithQuery search, Post (unauth/201/400), Put (200/404), Delete (204/404/403)
- KegApiTests: xUnit tests with GetAll, GetById (found/not found), GetAll_WithQuery search, Post (unauth/201/400), Put (200/404), Delete (204/404/403)
- WarehouseLocationApiTests: xUnit tests with GetAll, GetById (found/not found), Post (unauth/201/400), Put (200/404), Delete (204/404/403)
- StockEntryApiTests: xUnit tests with GetAll, GetById (found/not found), Post (unauth/201/400), Put (200/404), Delete (204/404/403)
- EmployeeApiTests: xUnit tests with GetAll, GetById (found/not found), Post (unauth/201/400), Put (200/404), Delete (204/404/403)

## EF Configuration
- BreweryWarehouseDbContext: Data/BreweryWarehouseDbContext.cs with DbSets for BeerStyle, Can, Keg, StockEntry, WarehouseLocation, Employee, TPH mapping for Container hierarchy, and DI registration using SqlServer
- Soft delete: Container and BeerStyle use DeletedAt (DateTime?) for soft deletion
- Localization: Program.cs configures RequestLocalization with hr default and en-US fallback cultures
- DatabaseSeeder: Data/DatabaseSeeder.cs seeds sample data on startup
- EF migrations: stored in BreweryWarehouse.Web/Migrations/ with InitialCreate as the first migration; RestrictStockEntryForeignKeys migration changes StockEntry.ContainerId and StockEntry.LocationId from CASCADE to RESTRICT
- Delete behavior: Can.BeerStyleId, Keg.BeerStyleId, StockEntry.ContainerId, StockEntry.LocationId all use DeleteBehavior.Restrict; Attachment.StockEntryId uses Cascade (intentional); Employee.AppUserId uses SetNull
- WarehouseLocationController.Delete: pre-checks StockEntries.Any() before attempting delete; shows TempData["Error"] with count if blocked; also catches DbUpdateException as safety net; redirects to Details on error
- StockEntryController.Delete: cleans up physical attachment files (wwwroot/uploads/stock-entries/{id}/) before DB delete; file I/O errors are logged at Warning but don't abort the operation
- StockEntryController.UploadAttachment: server-side validation — max 10 MB, allowed content types: image/jpeg, image/png, image/gif, image/webp, application/pdf, Word, Excel
- Validation: wwwroot/js/site.js sets global jQuery validation to trigger on blur and disables onkeyup
- Identity: BreweryWarehouseDbContext extends IdentityDbContext<AppUser>; roles Admin and WarehouseManager seeded via IdentitySeeder on startup

## Routing
- BeerStyleController: /beer-styles and /beer-styles/{id:int}/detail
- CanController: /cans and /cans/{id:int}/info
- KegController: /kegs and /kegs/{id:int}/info
- WarehouseLocationController: /locations and /locations/{id:int}/view
- BeerStyleApiController: GET/POST /api/beer-styles, GET/PUT/DELETE /api/beer-styles/{id}
- CanApiController: GET/POST /api/cans, GET/PUT/DELETE /api/cans/{id}
- KegApiController: GET/POST /api/kegs, GET/PUT/DELETE /api/kegs/{id}
- WarehouseLocationApiController: GET/POST /api/locations, GET/PUT/DELETE /api/locations/{id}
- StockEntryApiController: GET/POST /api/stock-entries, GET/PUT/DELETE /api/stock-entries/{id}
- EmployeeApiController: GET/POST /api/employees, GET/PUT/DELETE /api/employees/{id}
- GlobalSearchController: GET /search/global?q= (returns JSON, requires auth, min 2 chars)
- UserManagementController: GET /UserManagement (Index), GET/POST /UserManagement/Edit/{id}
- MCP Server: /mcp (SSE + HTTP transport, API-key auth via X-Mcp-Api-Key header, Mcp__ApiKey app setting)

## MCP Server

Endpoint: `/mcp` — Model Context Protocol server hosted inside the ASP.NET Core app (same process/deployment).
Packages: `ModelContextProtocol` + `ModelContextProtocol.AspNetCore` v0.3.0-preview.2.
Auth: `X-Mcp-Api-Key` request header validated against the `Mcp__ApiKey` App Service Application Setting. Missing or wrong key → 401.
Tool files: `BreweryWarehouse.Web/McpTools/` — one class per entity + `McpApiKeyAuthHandler.cs`.

### Exposed tools

| Tool name | Entity | Operation |
|---|---|---|
| `list_beer_styles` | BeerStyle | GetAll() — excludes soft-deleted |
| `get_beer_style` | BeerStyle | GetById(id) |
| `create_beer_style` | BeerStyle | Add() — full write proof |
| `list_cans` | Can | GetAll() |
| `get_can` | Can | GetById(id) |
| `list_kegs` | Keg | GetAll() |
| `get_keg` | Keg | GetById(id) |
| `list_warehouse_locations` | WarehouseLocation | GetAll() |
| `get_warehouse_location` | WarehouseLocation | GetById(id) |
| `list_stock_entries` | StockEntry | GetAll() |
| `get_stock_entry` | StockEntry | GetById(id) |
| `list_employees` | Employee | GetAll() |
| `get_employee` | Employee | GetById(id) |

### Connecting from an agentic IDE

Add to your MCP client config (e.g. Claude Code `mcp.json`, VS Code `settings.json` MCP block):

```json
{
  "mcpServers": {
    "brewery-warehouse": {
      "url": "https://brewery-warehouse-hagy.azurewebsites.net/mcp",
      "headers": {
        "X-Mcp-Api-Key": "<value-of-Mcp__ApiKey-setting>"
      }
    }
  }
}
```

For local development: `"url": "https://localhost:7001/mcp"` (or whichever port the app runs on).
Set the key locally via `dotnet user-secrets set "Mcp__ApiKey" "<your-key>"` or in environment as `Mcp__ApiKey=<value>`.

## E2E Testing

Project: `BreweryWarehouse.E2ETests` — Playwright browser automation with xUnit. Intentionally separate from `BreweryWarehouse.Tests` (which is fast and in-memory). E2E tests require a live running app and are not run as part of the normal CI test pass.

### One-time setup (per machine)
```bash
# Build the project to generate playwright.ps1
dotnet build BreweryWarehouse.E2ETests/BreweryWarehouse.E2ETests.csproj

# Install Chromium (only chromium — no need for Firefox/WebKit)
pwsh BreweryWarehouse.E2ETests/bin/Debug/net8.0/playwright.ps1 install chromium
```

### Running the E2E tests
```bash
# 1. Start the app in a separate terminal
dotnet run --project BreweryWarehouse.Web

# 2. In another terminal, run only the E2E project
dotnet test BreweryWarehouse.E2ETests/BreweryWarehouse.E2ETests.csproj
```

The test targets `http://localhost:5289` (http profile from `launchSettings.json`). It logs in as `admin@brewery.com` / `Admin123!` (seeded by `IdentitySeeder`) — do not use the Google OAuth flow in E2E tests (bot detection, external dependency).

### Test scenario
`BeerStyleE2ETests.BeerStyle_FullLifecycle_10Steps` — 10 clearly-commented steps:
1. Navigate to login page
2. Log in with seeded admin credentials
3. Navigate to BeerStyle Index
4. Create a new BeerStyle ("Playwright Test Ale")
5. Assert it appears in the Index list
6. Navigate to its Details page
7. Edit the Description field, save
8. Assert the updated Description appears in Details
9. Delete through the confirmation flow
10. Assert it no longer appears in Index, then log out

## Self-Update Rule
After completing any task that adds, removes or modifies classes, methods, 
or project structure — update this file under the relevant section to reflect 
the current state of the project. Keep entries concise, one line per item.

## Current Classes
- Container: Id (int, [Key]), SLCode (string), BestBefore (DateTime), DeletedAt (DateTime?)
- BeerStyle: Id (int, [Key]), Name (string), Description (string), AlcoholPercentage (double), IBU (int), ColorEBC (double), Category (BeerCategory), DeletedAt (DateTime?), Cans (virtual ICollection<Can>), Kegs (virtual ICollection<Keg>)
- Can: inherits Container (Id, SLCode, BestBefore), Size (CanSize), Barcode (string), PackagingDate (DateTime), StockEntries (virtual ICollection<StockEntry>), BeerStyleId (int), BeerStyle (virtual BeerStyle, [ForeignKey("BeerStyleId")])
- Keg: inherits Container (Id, SLCode, BestBefore), Material (KegMaterial), HeadType (KegHeadType), VolumeInLitres (int: 20 or 30), SerialNumber (string), LastInspection (DateTime), StockEntries (virtual ICollection<StockEntry>), BeerStyleId (int), BeerStyle (virtual BeerStyle, [ForeignKey("BeerStyleId")])
- StockEntry: Id (int, [Key]), ContainerId (int), Container (virtual Container, [ForeignKey("ContainerId")]), LocationId (int), Location (virtual WarehouseLocation, [ForeignKey("LocationId")]), Quantity (int), DateReceived (DateTime), DateModified (DateTime), Notes (string)
- Attachment: BreweryWarehouse.Model/Attachment.cs — Id (int, [Key]), StockEntryId (int), StockEntry (virtual StockEntry), FileName (string), FilePath (string), ContentType (string), FileSize (long), CreatedAt (DateTime)
- WarehouseLocation: Id (int, [Key]), LocationCode (string), Aisle (string), Shelf (int), MaxCapacity (int), Description (string), StockEntries (virtual ICollection<StockEntry>)
- Employee: Id (int, [Key]), FirstName (string), LastName (string), Email (string), Role (string), DateHired (DateTime), IsActive (bool), AppUserId (string?, nullable FK to AppUser — configured via fluent API in DbContext, OnDelete SetNull)
- EnumExtensions: GetDescription(this Enum) helper for enum DescriptionAttribute labels
- DataSeeder: static class with Seed(out List<BeerStyle>, out List<Can>, out List<Keg>, out List<WarehouseLocation>, out List<StockEntry>, out List<Employee>) using expanded, edge-case-heavy sample data for UI inspection
- AppUser: extends IdentityUser with OIB (string, 11 digits) and JMBG (string, 13 digits)
- IdentitySeeder: seeds Admin and WarehouseManager roles plus default admin@brewery.com account
- DashboardViewModel: TotalCans, TotalKegs, TotalLocations, TotalBeerStyles, TotalStockUnits (sum of all StockEntry.Quantity), ExpiringCans (List<Can>), ExpiringKegs (List<Keg>), Locations (List<WarehouseLocation>)
- BeerStyleCreateModel: Models/BeerStyleCreateModel.cs — Name, Description, AlcoholPercentage, IBU, ColorEBC, Category with DataAnnotations
- BeerStyleEditModel: Models/BeerStyleEditModel.cs — Id, Name, Description, AlcoholPercentage, IBU, ColorEBC, Category with DataAnnotations
- CanCreateModel: Models/CanCreateModel.cs — SLCode, BestBefore, Size, Barcode, PackagingDate, BeerStyleId, BeerStyleName with DataAnnotations
- CanEditModel: Models/CanEditModel.cs — Id, SLCode, BestBefore, Size, Barcode, PackagingDate, BeerStyleId, BeerStyleName with DataAnnotations
- KegCreateModel: Models/KegCreateModel.cs — SLCode, BestBefore, Material, HeadType, VolumeInLitres, SerialNumber, LastInspection, BeerStyleId, BeerStyleName with DataAnnotations
- KegEditModel: Models/KegEditModel.cs — Id, SLCode, BestBefore, Material, HeadType, VolumeInLitres, SerialNumber, LastInspection, BeerStyleId, BeerStyleName with DataAnnotations
- WarehouseLocationCreateModel: Models/WarehouseLocationCreateModel.cs — LocationCode, Aisle, Shelf, MaxCapacity, Description (string?) with DataAnnotations
- WarehouseLocationEditModel: Models/WarehouseLocationEditModel.cs — Id, LocationCode, Aisle, Shelf, MaxCapacity, Description with DataAnnotations
- StockEntryCreateModel: Models/StockEntryCreateModel.cs — ContainerId, LocationId, Quantity, DateReceived, Notes with DataAnnotations
- StockEntryEditModel: Models/StockEntryEditModel.cs — Id, ContainerId, LocationId, Quantity, DateReceived, Notes with DataAnnotations
- EmployeeCreateModel: Models/EmployeeCreateModel.cs — FirstName, LastName, Email, Role, DateHired, IsActive with DataAnnotations
- EmployeeEditModel: Models/EmployeeEditModel.cs — Id, FirstName, LastName, Email, Role, DateHired, IsActive, AppUserId (string?) with DataAnnotations
- DatePickerModel: Models/DatePickerModel.cs — FieldName, Value, Label, IsRequired
- CanMockRepository: static seeded List<Can> with GetAll() and GetById(int id)
- KegMockRepository: static seeded List<Keg> with GetAll() and GetById(int id)
- WarehouseLocationMockRepository: static seeded List<WarehouseLocation> with GetAll() and GetById(int id)
- StockEntryMockRepository: static seeded List<StockEntry> with GetAll() and GetById(int id)
- EmployeeMockRepository: static seeded List<Employee> with GetAll() and GetById(int id)
- GlobalSearchController: Controllers/GlobalSearchController.cs with [Authorize] [Route("search")], GET /search/global?q= returning flat JSON array of {category, label, subtitle, url} — searches BeerStyle.Name, Can.SLCode/Barcode, Keg.SLCode/SerialNumber, WarehouseLocation.LocationCode, StockEntry.Container.SLCode/Location.LocationCode, Employee.FirstName/LastName, plus static page list; min 2 chars required; 5 results per entity; pages appear first, User Roles page only for Admin role; URLs via Url.Action
- UserManagementController: Controllers/UserManagementController.cs with [Authorize(Roles="Admin")], async Index() listing all users with roles, Edit(string id) GET/POST — injects UserManager&lt;AppUser&gt; and RoleManager&lt;IdentityRole&gt;; self-lockout guard on POST; logs role changes at Information level
- UserManagement Views: Views/UserManagement/Index.cshtml (user+role table with TempData success alert), Views/UserManagement/Edit.cshtml (role assign form, self-lockout disabled state)
- UserRoleListItem: Models/UserRoleListItem.cs — Id (string), Email (string), CurrentRole (string?)
- McpApiKeyHandler: McpTools/McpApiKeyAuthHandler.cs — McpApiKeyRequirement + IAuthorizationHandler; validates X-Mcp-Api-Key header against Mcp__ApiKey config; wired as "McpApiKey" policy
- BeerStyleTools: McpTools/BeerStyleTools.cs — [McpServerToolType]; list_beer_styles, get_beer_style, create_beer_style
- CanTools: McpTools/CanTools.cs — [McpServerToolType]; list_cans, get_can
- KegTools: McpTools/KegTools.cs — [McpServerToolType]; list_kegs, get_keg
- WarehouseLocationTools: McpTools/WarehouseLocationTools.cs — [McpServerToolType]; list_warehouse_locations, get_warehouse_location
- StockEntryTools: McpTools/StockEntryTools.cs — [McpServerToolType]; list_stock_entries, get_stock_entry
- EmployeeTools: McpTools/EmployeeTools.cs — [McpServerToolType]; list_employees, get_employee
- UserRoleEditModel: Models/UserRoleEditModel.cs — Id (string), Email (string), SelectedRole (string?)

## Current Enums
- BeerCategory: Lager, Ale, IPA, Stout, Wheat, Sour, Porter
- CanSize: Can033, Can044
- KegMaterial: Plastic, Metal
- KegHeadType: KeyHead, SHead

## Skill Routing
Read and follow the linked skill file whenever the task matches the trigger. Do not wait to be asked.

| Trigger | Skill file |
|---|---|
| Adding/modifying EF entities, creating migrations, updating DbContext, or working with repositories | `.claude/commands/ef-skill.md` |
| Creating a new Index/list view or its controller action | `.claude/commands/list-page-skill.md` |
| Creating CRUD ViewModels (CreateModel/EditModel), or Create/Edit/Delete controller actions and views | `.claude/commands/crud-forms-skill.md` |
| Adding or applying the custom date picker partial | `.claude/commands/datepicker-skill.md` |
| Adding AJAX search, autocomplete dropdowns, or any jQuery interaction | `.claude/commands/ajax-js-skill.md` |
| Creating or modifying the brewery load animation overlay | `.claude/commands/load-animation-skill.md` |
| Adding logging to new controllers, changing log levels, or diagnosing log output | `.claude/commands/logging-skill.md` |

## UX Sub-Agent
When generating or significantly modifying any Razor view (.cshtml), spawn a subagent using the Agent tool (subagent_type: claude) with the full contents of `.claude/agents/ux-agent.md` as its context. Hand off the specific view task; do not generate view code yourself.

## Logging
- Library: **Serilog** (`Serilog.AspNetCore` + `Serilog.Sinks.File`). Do not add a second logging library.
- File path is resolved in `Program.cs` at startup: `/home/LogFiles/Application/log-.txt` on Azure App Service Linux (persistent storage), `logs/log-.txt` locally. Console sink is configured via `appsettings.json`; file sink path is code-only so the runtime check applies.
- Rolling: daily, 14 files retained (bounded for Azure F1 1 GB cap).
- Log levels: `BreweryWarehouse.*` → Information; `Microsoft.AspNetCore` → Warning; `Microsoft.AspNetCore.Authorization` → **Information** (surfaces Google-OAuth no-role denial events without custom code); `Microsoft.EntityFrameworkCore` → Warning.
- Controller convention: log Warning on validation failure, Information on successful Create/Edit/Delete. No GET logging. Never log secrets (connection strings, passwords, OAuth tokens).
- See `.claude/commands/logging-skill.md` for full conventions.

## AI Integration
- Package: `OpenAI` v2.2.0 (official Microsoft/OpenAI SDK), pointed at DeepSeek's base URL (`https://api.deepseek.com`) — no DeepSeek-specific package needed
- Model: `deepseek-v4-flash` — do NOT use `deepseek-chat` (legacy alias, stops working 2026-07-24)
- `OpenAIClient` registered as `AddSingleton` in Program.cs; API key read from `IConfiguration["DeepSeek:ApiKey"]` (App Service setting `DeepSeek__ApiKey`, local dev via user-secrets)
- Feature: "AI Quick Add" on BeerStyle Create page — user describes a beer style in plain text, AI pre-fills the Create form via `BeerStyleController.AiQuickAdd` POST action (`/beer-styles/ai-quick-add`)
- Human-in-the-loop design: AI never writes directly to the DB — it returns a pre-populated `BeerStyleCreateModel` back to the Create view for user review and manual Save. This reuses the existing validated Create path and gives a human review step before any data is persisted.
- JSON output mode (`ChatResponseFormat.CreateJsonObjectFormat()`) used for reliable structured extraction; temperature 0, max 300 tokens
- Error handling: API failures (including 401 = empty prepaid balance) degrade gracefully to an empty Create form with a user-friendly TempData error message; logged at Warning
- Extension path: this pattern can be applied to other entities (Can, Keg, etc.) using their respective CreateModel and a matching AiQuickAdd action

## Deployment
- Hosted on Azure App Service (F1 Free, Linux, .NET 8): https://brewery-warehouse-hagy.azurewebsites.net
- Database: Azure SQL Database (free serverless tier), same region (swedencentral)
- Secrets (connection string under key `BreweryWarehouseDbContext`, Google OAuth) set via App Service Application Settings, never in appsettings.json
- Google OAuth redirect URI registered for both localhost (dev) and the azurewebsites.net domain (prod)
- DatabaseSeeder and IdentitySeeder are wrapped in try/catch in Program.cs — a slow or failed cold-start DB connection logs an error but does NOT crash the app; the app starts and serves requests regardless
- Azure SQL serverless cold-start risk: free-tier database auto-pauses when idle; the connection string should include `Connection Timeout=60` and EF Core is configured with `EnableRetryOnFailure(maxRetryCount:5, maxRetryDelay:30s)` to handle transient resume delays automatically
- Role assignment for new users: Google OAuth logins get an `AspNetUsers` row but no role. Assign roles via the Admin-only `/UserManagement` page (see UserManagementController). The previous workaround of manual `INSERT INTO AspNetUserRoles` via Azure SQL Query editor is no longer needed.
- Known limitation: StockEntry attachment uploads to wwwroot are not guaranteed to survive a redeploy (not yet on Blob Storage)
- Upgrade path: F1 → B1 App Service tier is a portal-only change, no redeploy needed, for custom domain / no cold-start later