namespace BreweryWarehouse.Model;

public static class DataSeeder
{
    public static void Seed(
        out List<BeerStyle> beerStyles,
        out List<Can> cans,
        out List<Keg> kegs,
        out List<WarehouseLocation> warehouseLocations,
        out List<StockEntry> stockEntries,
        out List<Employee> employees)
    {
        beerStyles = new List<BeerStyle>
        {
            new BeerStyle
            {
                Id = 1,
                Name = "North Dock Pils",
                Description = "Crisp Czech-style pilsner with floral hops.",
                AlcoholPercentage = 5.1,
                IBU = 34,
                ColorEBC = 8.5,
                Category = BeerCategory.Lager
            },
            new BeerStyle
            {
                Id = 2,
                Name = "Harbor Haze",
                Description = "Juicy hazy IPA with tropical aroma.",
                AlcoholPercentage = 6.3,
                IBU = 42,
                ColorEBC = 14.0,
                Category = BeerCategory.IPA
            },
            new BeerStyle
            {
                Id = 3,
                Name = "Midnight Forge",
                Description = "Roasty dry stout with dark chocolate notes.",
                AlcoholPercentage = 5.8,
                IBU = 36,
                ColorEBC = 80.0,
                Category = BeerCategory.Stout
            }
        };

        cans = new List<Can>
        {
            new Can { Id = 1, SLCode = "SL101", BestBefore = new DateTime(2026, 10, 31), Size = CanSize.Can033, Barcode = "5901001000010", PackagingDate = new DateTime(2026, 3, 10), BeerStyle = beerStyles[0] },
            new Can { Id = 2, SLCode = "SL102", BestBefore = new DateTime(2026, 11, 5), Size = CanSize.Can044, Barcode = "5901001000027", PackagingDate = new DateTime(2026, 3, 11), BeerStyle = beerStyles[0] },
            new Can { Id = 3, SLCode = "SL103", BestBefore = new DateTime(2026, 11, 12), Size = CanSize.Can033, Barcode = "5901001000034", PackagingDate = new DateTime(2026, 3, 12), BeerStyle = beerStyles[0] },

            new Can { Id = 4, SLCode = "SL104", BestBefore = new DateTime(2026, 9, 20), Size = CanSize.Can033, Barcode = "5901001000041", PackagingDate = new DateTime(2026, 3, 8), BeerStyle = beerStyles[1] },
            new Can { Id = 5, SLCode = "SL105", BestBefore = new DateTime(2026, 9, 24), Size = CanSize.Can044, Barcode = "5901001000058", PackagingDate = new DateTime(2026, 3, 9), BeerStyle = beerStyles[1] },
            new Can { Id = 6, SLCode = "SL106", BestBefore = new DateTime(2026, 10, 1), Size = CanSize.Can033, Barcode = "5901001000065", PackagingDate = new DateTime(2026, 3, 10), BeerStyle = beerStyles[1] },

            new Can { Id = 7, SLCode = "SL107", BestBefore = new DateTime(2026, 12, 15), Size = CanSize.Can033, Barcode = "5901001000072", PackagingDate = new DateTime(2026, 3, 14), BeerStyle = beerStyles[2] },
            new Can { Id = 8, SLCode = "SL108", BestBefore = new DateTime(2026, 12, 20), Size = CanSize.Can044, Barcode = "5901001000089", PackagingDate = new DateTime(2026, 3, 15), BeerStyle = beerStyles[2] },
            new Can { Id = 9, SLCode = "SL109", BestBefore = new DateTime(2026, 12, 27), Size = CanSize.Can033, Barcode = "5901001000096", PackagingDate = new DateTime(2026, 3, 16), BeerStyle = beerStyles[2] }
        };

        kegs = new List<Keg>
        {
            new Keg { Id = 10, SLCode = "SL201", BestBefore = new DateTime(2026, 9, 30), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-LAG-001", LastInspection = new DateTime(2026, 1, 12), BeerStyle = beerStyles[0] },
            new Keg { Id = 11, SLCode = "SL202", BestBefore = new DateTime(2026, 10, 8), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-LAG-002", LastInspection = new DateTime(2026, 1, 20), BeerStyle = beerStyles[0] },
            new Keg { Id = 12, SLCode = "SL203", BestBefore = new DateTime(2026, 10, 15), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-LAG-003", LastInspection = new DateTime(2026, 2, 2), BeerStyle = beerStyles[0] },

            new Keg { Id = 13, SLCode = "SL204", BestBefore = new DateTime(2026, 8, 25), Material = KegMaterial.Plastic, HeadType = KegHeadType.KeyHead, VolumeInLitres = 20, SerialNumber = "KG-IPA-001", LastInspection = new DateTime(2026, 1, 18), BeerStyle = beerStyles[1] },
            new Keg { Id = 14, SLCode = "SL205", BestBefore = new DateTime(2026, 9, 1), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-IPA-002", LastInspection = new DateTime(2026, 2, 4), BeerStyle = beerStyles[1] },
            new Keg { Id = 15, SLCode = "SL206", BestBefore = new DateTime(2026, 9, 8), Material = KegMaterial.Plastic, HeadType = KegHeadType.KeyHead, VolumeInLitres = 20, SerialNumber = "KG-IPA-003", LastInspection = new DateTime(2026, 2, 11), BeerStyle = beerStyles[1] },

            new Keg { Id = 16, SLCode = "SL207", BestBefore = new DateTime(2026, 11, 30), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-STO-001", LastInspection = new DateTime(2026, 1, 28), BeerStyle = beerStyles[2] },
            new Keg { Id = 17, SLCode = "SL208", BestBefore = new DateTime(2026, 12, 5), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-STO-002", LastInspection = new DateTime(2026, 2, 6), BeerStyle = beerStyles[2] },
            new Keg { Id = 18, SLCode = "SL209", BestBefore = new DateTime(2026, 12, 10), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-STO-003", LastInspection = new DateTime(2026, 2, 15), BeerStyle = beerStyles[2] }
        };

        foreach (Can can in cans)
        {
            can.BeerStyle.Cans.Add(can);
        }

        foreach (Keg keg in kegs)
        {
            keg.BeerStyle.Kegs.Add(keg);
        }

        warehouseLocations = new List<WarehouseLocation>
        {
            new WarehouseLocation
            {
                Id = 1,
                LocationCode = "A-01-3",
                Aisle = "A",
                Shelf = 3,
                MaxCapacity = 240,
                Description = "Cold room shelf for packaged cans"
            },
            new WarehouseLocation
            {
                Id = 2,
                LocationCode = "B-02-1",
                Aisle = "B",
                Shelf = 1,
                MaxCapacity = 120,
                Description = "Ground shelf near loading dock"
            },
            new WarehouseLocation
            {
                Id = 3,
                LocationCode = "C-03-2",
                Aisle = "C",
                Shelf = 2,
                MaxCapacity = 90,
                Description = "Keg zone with quick dispatch access"
            }
        };

        List<Container> containers = cans.Cast<Container>().Concat(kegs.Cast<Container>()).ToList();
        stockEntries = new List<StockEntry>();
        int stockEntryId = 1;

        for (int i = 0; i < containers.Count; i++)
        {
            WarehouseLocation location = warehouseLocations[i % warehouseLocations.Count];
            DateTime receivedDate = new DateTime(2026, 3, 18).AddDays(i % 7);
            int quantity = containers[i] is Can ? 24 + ((i % 3) * 12) : 2 + (i % 4);

            StockEntry stockEntry = new StockEntry
            {
                Id = stockEntryId++,
                Container = containers[i],
                Location = location,
                Quantity = quantity,
                DateReceived = receivedDate,
                DateModified = receivedDate.AddDays(2),
                Notes = containers[i] is Can
                    ? "Palletized can stock ready for retail orders"
                    : "Keg stock for taproom and wholesale dispatch"
            };

            stockEntries.Add(stockEntry);
            location.StockEntries.Add(stockEntry);

            if (containers[i] is Can stockedCan)
            {
                stockedCan.StockEntries.Add(stockEntry);
            }
            else if (containers[i] is Keg stockedKeg)
            {
                stockedKeg.StockEntries.Add(stockEntry);
            }
        }

        employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FirstName = "Marta",
                LastName = "Wojcik",
                Email = "marta.wojcik@brewerywarehouse.local",
                Role = "Warehouse Manager",
                DateHired = new DateTime(2022, 4, 12),
                IsActive = true
            },
            new Employee
            {
                Id = 2,
                FirstName = "Liam",
                LastName = "Nowak",
                Email = "liam.nowak@brewerywarehouse.local",
                Role = "Forklift Operator",
                DateHired = new DateTime(2023, 9, 4),
                IsActive = true
            },
            new Employee
            {
                Id = 3,
                FirstName = "Ewa",
                LastName = "Kaczmarek",
                Email = "ewa.kaczmarek@brewerywarehouse.local",
                Role = "Quality Technician",
                DateHired = new DateTime(2021, 11, 8),
                IsActive = true
            }
        };
    }
}
