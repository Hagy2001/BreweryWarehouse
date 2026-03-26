## Project: BreweryWarehouse
A craft brewery warehouse management system built with ASP.NET Core MVC / C# .NET 8.

## Solution Structure
- `BreweryWarehouse.Model` — class library, all domain classes
- `BreweryWarehouse.Console` — LINQ queries and seed data
- `BreweryWarehouse.Web` — ASP.NET Core MVC web app (future labs)

## Conventions
- Namespace: BreweryWarehouse.Model for all model classes
- No public fields, use public auto-properties only
- Initialize all List<> properties in constructors
- All classes must be public

## Self-Update Rule
After completing any task that adds, removes or modifies classes, methods, 
or project structure — update this file under the relevant section to reflect 
the current state of the project. Keep entries concise, one line per item.

## Current Classes
- Container: Id (int), SLCode (string), BestBefore (DateTime)
- BeerStyle: Id (int), Name (string), Description (string), AlcoholPercentage (double), IBU (int), ColorEBC (double), Category (BeerCategory), Cans (List<Can>), Kegs (List<Keg>)
- Can: inherits Container (Id, SLCode, BestBefore)
- Keg: inherits Container (Id, SLCode, BestBefore)

## Current Enums
- BeerCategory: Lager, Ale, IPA, Stout, Wheat, Sour, Porter
- CanSize: Can033, Can044
- KegMaterial: Plastic, Metal
- KegHeadType: KegHead, SHead