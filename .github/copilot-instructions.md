## Project: BreweryWarehouse
A craft brewery warehouse management system built with ASP.NET Core MVC / C# .NET 8.

## Solution Structure
- `BreweryWarehouse.Model` — class library, all domain classes
- `BreweryWarehouse.Console` — LINQ queries and seed data (Lab 1)
- `BreweryWarehouse.Web` — ASP.NET Core MVC web app (active from Lab 2)

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
- BeerStyleMockRepository: Repositories/BeerStyleMockRepository.cs with GetAll() and GetById(int id) from DataSeeder
- BeerStyleController: Controllers/BeerStyleController.cs with Index() and Details(int id)
- BeerStyle Views: Views/BeerStyle/Index.cshtml and Views/BeerStyle/Details.cshtml
- Shared Layout: Views/Shared/_Layout.cshtml uses brewery sidebar navigation and shared site.css theme

## Sub-Agent
A UX sub-agent is defined at `.github/agents/ux-agent.agent.md` (model: gemini-3.1-pro).
The main agent must hand off to it when generating any UI/View code by referencing it by name.
Log of sub-agent calls is maintained in `lab-2/agent_log.txt` as proof.

## Self-Update Rule
After completing any task that adds, removes or modifies classes, methods, 
or project structure — update this file under the relevant section to reflect 
the current state of the project. Keep entries concise, one line per item.

## Current Classes
- Container: Id (int), SLCode (string), BestBefore (DateTime)
- BeerStyle: Id (int), Name (string), Description (string), AlcoholPercentage (double), IBU (int), ColorEBC (double), Category (BeerCategory), Cans (List<Can>), Kegs (List<Keg>)
- Can: inherits Container (Id, SLCode, BestBefore), Size (CanSize), Barcode (string), PackagingDate (DateTime), StockEntries (List<StockEntry>), BeerStyle (BeerStyle)
- Keg: inherits Container (Id, SLCode, BestBefore), Material (KegMaterial), HeadType (KegHeadType), VolumeInLitres (int: 20 or 30), SerialNumber (string), LastInspection (DateTime), StockEntries (List<StockEntry>), BeerStyle (BeerStyle)
- StockEntry: Id (int), Container (Container), Location (WarehouseLocation), Quantity (int), DateReceived (DateTime), DateModified (DateTime), Notes (string)
- WarehouseLocation: Id (int), LocationCode (string), Aisle (string), Shelf (int), MaxCapacity (int), Description (string), StockEntries (List<StockEntry>)
- Employee: Id (int), FirstName (string), LastName (string), Email (string), Role (string), DateHired (DateTime), IsActive (bool)
- EnumExtensions: GetDescription(this Enum) helper for enum DescriptionAttribute labels
- DataSeeder: static class with Seed(out List<BeerStyle>, out List<Can>, out List<Keg>, out List<WarehouseLocation>, out List<StockEntry>, out List<Employee>)

## Current Enums
- BeerCategory: Lager, Ale, IPA, Stout, Wheat, Sour, Porter
- CanSize: Can033, Can044
- KegMaterial: Plastic, Metal
- KegHeadType: KeyHead, SHead