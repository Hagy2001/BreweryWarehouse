## Project: BreweryWarehouse
A craft brewery warehouse management system built with ASP.NET Core MVC / C# .NET 8.

## Solution Structure
- `BreweryWarehouse.Model` — class library, all domain classes
- `BreweryWarehouse.Console` — LINQ queries and seed data (Lab 1)
- `BreweryWarehouse.Web` — ASP.NET Core MVC web app (active from Lab 2)
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
- EmployeeController: Controllers/EmployeeController.cs with [Authorize], Index(), Details(int id), Create, Edit, Delete, Search(string q)
- Employee Views: Views/Employee/Index.cshtml, Views/Employee/Details.cshtml, Views/Employee/Create.cshtml, Views/Employee/Edit.cshtml, Views/Employee/_CreateOrEdit.cshtml (bw-badge--active/inactive status badges in site.css)
- HomeController: Controllers/HomeController.cs with [Authorize], BeerStyleRepository, CanRepository, KegRepository, WarehouseLocationRepository injection, dashboard Index() projection, Privacy(), Error()
- Home Dashboard View: Views/Home/Index.cshtml with KPI cards, expiring combined table, and stock-by-location utilization bars
- Shared Layout: Views/Shared/_Layout.cshtml uses brewery sidebar navigation and shared site.css theme
- DatePicker Partial: Views/Shared/_DatePicker.cshtml for custom date inputs
- BeerStyle Autocomplete Partial: Views/Shared/_BeerStyleAutocomplete.cshtml for AJAX beer style lookup inputs
- AuthService: Services/AuthService.cs with ValidateCredentials(email,password) and CreatePrincipal(email)
- AuthController: Controllers/AuthController.cs with Login(GET/POST) and Logout(POST)
- LoginViewModel: Models/LoginViewModel.cs with Required/EmailAddress validation
- Login View: Views/Auth/Login.cshtml standalone login page without shared layout

## EF Configuration
- BreweryWarehouseDbContext: Data/BreweryWarehouseDbContext.cs with DbSets for BeerStyle, Can, Keg, StockEntry, WarehouseLocation, Employee, TPH mapping for Container hierarchy, and DI registration using SqlServer
- Soft delete: Container and BeerStyle use DeletedAt (DateTime?) for soft deletion
- Localization: Program.cs configures RequestLocalization with hr and en-US cultures, default hr
- DatabaseSeeder: Data/DatabaseSeeder.cs seeds sample data on startup
- EF migrations: stored in BreweryWarehouse.Web/Migrations/ with InitialCreate as the first migration

## Routing
- BeerStyleController: /beer-styles and /beer-styles/{id:int}/detail
- CanController: /cans and /cans/{id:int}/info
- KegController: /kegs and /kegs/{id:int}/info
- WarehouseLocationController: /locations and /locations/{id:int}/view

## Sub-Agent
A UX sub-agent is defined at `.github/agents/ux-agent.agent.md` (model: gemini-3.1-pro).
The main agent must hand off to it when generating any UI/View code by referencing it by name.

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
- WarehouseLocation: Id (int, [Key]), LocationCode (string), Aisle (string), Shelf (int), MaxCapacity (int), Description (string), StockEntries (virtual ICollection<StockEntry>)
- Employee: Id (int, [Key]), FirstName (string), LastName (string), Email (string), Role (string), DateHired (DateTime), IsActive (bool)
- EnumExtensions: GetDescription(this Enum) helper for enum DescriptionAttribute labels
- DataSeeder: static class with Seed(out List<BeerStyle>, out List<Can>, out List<Keg>, out List<WarehouseLocation>, out List<StockEntry>, out List<Employee>) using expanded, edge-case-heavy sample data for UI inspection
- AuthService: mock credential validator and ClaimsPrincipal creator for admin@brewery.com
- LoginViewModel: Email and Password with DataAnnotations validation
- DashboardViewModel: TotalCans, TotalKegs, TotalLocations, TotalBeerStyles, ExpiringCans (List<Can>), ExpiringKegs (List<Keg>), Locations (List<WarehouseLocation>)
- BeerStyleCreateModel: Models/BeerStyleCreateModel.cs — Name, Description, AlcoholPercentage, IBU, ColorEBC, Category with DataAnnotations
- BeerStyleEditModel: Models/BeerStyleEditModel.cs — Id, Name, Description, AlcoholPercentage, IBU, ColorEBC, Category with DataAnnotations
- CanCreateModel: Models/CanCreateModel.cs — SLCode, BestBefore, Size, Barcode, PackagingDate, BeerStyleId, BeerStyleName with DataAnnotations
- CanEditModel: Models/CanEditModel.cs — Id, SLCode, BestBefore, Size, Barcode, PackagingDate, BeerStyleId, BeerStyleName with DataAnnotations
- KegCreateModel: Models/KegCreateModel.cs — SLCode, BestBefore, Material, HeadType, VolumeInLitres, SerialNumber, LastInspection, BeerStyleId, BeerStyleName with DataAnnotations
- KegEditModel: Models/KegEditModel.cs — Id, SLCode, BestBefore, Material, HeadType, VolumeInLitres, SerialNumber, LastInspection, BeerStyleId, BeerStyleName with DataAnnotations
- WarehouseLocationCreateModel: Models/WarehouseLocationCreateModel.cs — LocationCode, Aisle, Shelf, MaxCapacity, Description with DataAnnotations
- WarehouseLocationEditModel: Models/WarehouseLocationEditModel.cs — Id, LocationCode, Aisle, Shelf, MaxCapacity, Description with DataAnnotations
- StockEntryCreateModel: Models/StockEntryCreateModel.cs — ContainerId, LocationId, Quantity, DateReceived, Notes with DataAnnotations
- StockEntryEditModel: Models/StockEntryEditModel.cs — Id, ContainerId, LocationId, Quantity, DateReceived, Notes with DataAnnotations
- EmployeeCreateModel: Models/EmployeeCreateModel.cs — FirstName, LastName, Email, Role, DateHired, IsActive with DataAnnotations
- EmployeeEditModel: Models/EmployeeEditModel.cs — Id, FirstName, LastName, Email, Role, DateHired, IsActive with DataAnnotations
- DatePickerModel: Models/DatePickerModel.cs — FieldName, Value, Label, IsRequired
- CanMockRepository: static seeded List<Can> with GetAll() and GetById(int id)
- KegMockRepository: static seeded List<Keg> with GetAll() and GetById(int id)
- WarehouseLocationMockRepository: static seeded List<WarehouseLocation> with GetAll() and GetById(int id)
- StockEntryMockRepository: static seeded List<StockEntry> with GetAll() and GetById(int id)
- EmployeeMockRepository: static seeded List<Employee> with GetAll() and GetById(int id)

## Current Enums
- BeerCategory: Lager, Ale, IPA, Stout, Wheat, Sour, Porter
- CanSize: Can033, Can044
- KegMaterial: Plastic, Metal
- KegHeadType: KeyHead, SHead