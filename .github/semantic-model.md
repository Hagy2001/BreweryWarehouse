1. **Overview** — BreweryWarehouse is a craft brewery warehouse management app built with ASP.NET Core MVC on .NET 8, using EF Core for the database layer.

2. **Entities**
- BeerStyle: table BeerStyle; PK Id (int); properties Id (int), Name (string), Description (string), AlcoholPercentage (double), IBU (int), ColorEBC (double), Category (BeerCategory), Cans (ICollection<Can>), Kegs (ICollection<Keg>); enums BeerCategory
- Container: table Container (TPH); PK Id (int); properties Id (int), SLCode (string), BestBefore (DateTime)
- Can: table Container (TPH); PK Id (int); properties Id (int), SLCode (string), BestBefore (DateTime), Size (CanSize), Barcode (string), PackagingDate (DateTime), StockEntries (ICollection<StockEntry>), BeerStyleId (int), BeerStyle (BeerStyle); enums CanSize
- Keg: table Container (TPH); PK Id (int); properties Id (int), SLCode (string), BestBefore (DateTime), Material (KegMaterial), HeadType (KegHeadType), VolumeInLitres (int), SerialNumber (string), LastInspection (DateTime), StockEntries (ICollection<StockEntry>), BeerStyleId (int), BeerStyle (BeerStyle); enums KegMaterial, KegHeadType
- StockEntry: table StockEntry; PK Id (int); properties Id (int), ContainerId (int), Container (Container), LocationId (int), Location (WarehouseLocation), Quantity (int), DateReceived (DateTime), DateModified (DateTime), Notes (string)
- WarehouseLocation: table WarehouseLocation; PK Id (int); properties Id (int), LocationCode (string), Aisle (string), Shelf (int), MaxCapacity (int), Description (string), StockEntries (ICollection<StockEntry>)
- Employee: table Employee; PK Id (int); properties Id (int), FirstName (string), LastName (string), Email (string), Role (string), DateHired (DateTime), IsActive (bool)

3. **Relationships**
- BeerStyle 1->N Can via Can.BeerStyleId
- BeerStyle 1->N Keg via Keg.BeerStyleId
- Container 1->N StockEntry via StockEntry.ContainerId
- WarehouseLocation 1->N StockEntry via StockEntry.LocationId

4. **Enums**
- BeerCategory: Lager, Ale, IPA, Stout, Wheat, Sour, Porter
- CanSize: Can033, Can044
- KegMaterial: Plastic, Metal
- KegHeadType: KeyHead, SHead

5. **Inheritance** — Can and Keg inherit Container using TPH (Table-Per-Hierarchy), stored in a single Container table with a Discriminator column.

6. **EF Configuration notes** — DeleteBehavior.Restrict is set on Can.BeerStyleId and Keg.BeerStyleId to avoid cascade delete conflicts.
