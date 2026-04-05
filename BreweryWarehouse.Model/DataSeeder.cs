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
            },
            new BeerStyle
            {
                Id = 4,
                Name = "Copper Trail Amber",
                Description = "Balanced amber ale with caramel malt and citrus finish.",
                AlcoholPercentage = 5.4,
                IBU = 28,
                ColorEBC = 32.0,
                Category = BeerCategory.Ale
            },
            new BeerStyle
            {
                Id = 5,
                Name = "Pier Wheat",
                Description = "Refreshing wheat beer with clove and banana aroma.",
                AlcoholPercentage = 4.9,
                IBU = 18,
                ColorEBC = 10.0,
                Category = BeerCategory.Wheat
            },
            new BeerStyle
            {
                Id = 6,
                Name = "Sour Harbor",
                Description = "Bright kettle sour with stone-fruit character.",
                AlcoholPercentage = 4.6,
                IBU = 8,
                ColorEBC = 7.0,
                Category = BeerCategory.Sour
            },
            new BeerStyle
            {
                Id = 7,
                Name = "Old Crane Porter Reserve",
                Description = "Smooth porter with cocoa, toffee and subtle oak finish.",
                AlcoholPercentage = 6.9,
                IBU = 31,
                ColorEBC = 58.0,
                Category = BeerCategory.Porter
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
            new Can { Id = 9, SLCode = "SL109", BestBefore = new DateTime(2026, 12, 27), Size = CanSize.Can033, Barcode = "5901001000096", PackagingDate = new DateTime(2026, 3, 16), BeerStyle = beerStyles[2] },

            new Can { Id = 19, SLCode = "SL110", BestBefore = new DateTime(2026, 10, 20), Size = CanSize.Can033, Barcode = "5901001000102", PackagingDate = new DateTime(2026, 3, 17), BeerStyle = beerStyles[3] },
            new Can { Id = 20, SLCode = "SL111", BestBefore = new DateTime(2026, 10, 28), Size = CanSize.Can044, Barcode = "5901001000119", PackagingDate = new DateTime(2026, 3, 18), BeerStyle = beerStyles[3] },
            new Can { Id = 21, SLCode = "SL112", BestBefore = new DateTime(2026, 11, 7), Size = CanSize.Can033, Barcode = "5901001000126", PackagingDate = new DateTime(2026, 3, 19), BeerStyle = beerStyles[3] },

            new Can { Id = 22, SLCode = "SL113", BestBefore = new DateTime(2026, 9, 18), Size = CanSize.Can033, Barcode = "5901001000133", PackagingDate = new DateTime(2026, 3, 20), BeerStyle = beerStyles[4] },
            new Can { Id = 23, SLCode = "SL114", BestBefore = new DateTime(2026, 9, 26), Size = CanSize.Can044, Barcode = "5901001000140", PackagingDate = new DateTime(2026, 3, 21), BeerStyle = beerStyles[4] },
            new Can { Id = 24, SLCode = "SL115", BestBefore = new DateTime(2026, 10, 5), Size = CanSize.Can033, Barcode = "5901001000157", PackagingDate = new DateTime(2026, 3, 22), BeerStyle = beerStyles[4] },

            new Can { Id = 25, SLCode = "SL116", BestBefore = new DateTime(2026, 4, 2), Size = CanSize.Can033, Barcode = "5901001000164", PackagingDate = new DateTime(2026, 3, 23), BeerStyle = beerStyles[5] },
            new Can { Id = 26, SLCode = "SL117", BestBefore = new DateTime(2026, 4, 18), Size = CanSize.Can044, Barcode = "5901001000171", PackagingDate = new DateTime(2026, 3, 24), BeerStyle = beerStyles[5] },
            new Can { Id = 27, SLCode = "SL118", BestBefore = new DateTime(2026, 5, 2), Size = CanSize.Can033, Barcode = "5901001000188", PackagingDate = new DateTime(2026, 3, 25), BeerStyle = beerStyles[5] },

            new Can { Id = 37, SLCode = "SL119", BestBefore = new DateTime(2026, 5, 5), Size = CanSize.Can044, Barcode = "5901001000195", PackagingDate = new DateTime(2026, 3, 26), BeerStyle = beerStyles[6] },
            new Can { Id = 38, SLCode = "SL120", BestBefore = new DateTime(2026, 6, 12), Size = CanSize.Can033, Barcode = "5901001000201", PackagingDate = new DateTime(2026, 3, 27), BeerStyle = beerStyles[6] },
            new Can { Id = 39, SLCode = "SL121", BestBefore = new DateTime(2026, 7, 1), Size = CanSize.Can044, Barcode = "5901001000218", PackagingDate = new DateTime(2026, 3, 28), BeerStyle = beerStyles[6] }
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
            new Keg { Id = 18, SLCode = "SL209", BestBefore = new DateTime(2026, 12, 10), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-STO-003", LastInspection = new DateTime(2026, 2, 15), BeerStyle = beerStyles[2] },

            new Keg { Id = 28, SLCode = "SL210", BestBefore = new DateTime(2026, 10, 22), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-ALE-001", LastInspection = new DateTime(2026, 2, 20), BeerStyle = beerStyles[3] },
            new Keg { Id = 29, SLCode = "SL211", BestBefore = new DateTime(2026, 10, 29), Material = KegMaterial.Plastic, HeadType = KegHeadType.KeyHead, VolumeInLitres = 20, SerialNumber = "KG-ALE-002", LastInspection = new DateTime(2026, 2, 23), BeerStyle = beerStyles[3] },
            new Keg { Id = 30, SLCode = "SL212", BestBefore = new DateTime(2026, 11, 6), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-ALE-003", LastInspection = new DateTime(2026, 2, 26), BeerStyle = beerStyles[3] },

            new Keg { Id = 31, SLCode = "SL213", BestBefore = new DateTime(2026, 9, 16), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-WHE-001", LastInspection = new DateTime(2026, 3, 1), BeerStyle = beerStyles[4] },
            new Keg { Id = 32, SLCode = "SL214", BestBefore = new DateTime(2026, 9, 24), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-WHE-002", LastInspection = new DateTime(2026, 3, 3), BeerStyle = beerStyles[4] },
            new Keg { Id = 33, SLCode = "SL215", BestBefore = new DateTime(2026, 10, 2), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-WHE-003", LastInspection = new DateTime(2026, 3, 5), BeerStyle = beerStyles[4] },

            new Keg { Id = 34, SLCode = "SL216", BestBefore = new DateTime(2026, 3, 30), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-SOU-001", LastInspection = new DateTime(2026, 3, 7), BeerStyle = beerStyles[5] },
            new Keg { Id = 35, SLCode = "SL217", BestBefore = new DateTime(2026, 4, 14), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-SOU-002", LastInspection = new DateTime(2026, 3, 9), BeerStyle = beerStyles[5] },
            new Keg { Id = 36, SLCode = "SL218", BestBefore = new DateTime(2026, 5, 1), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-SOU-003", LastInspection = new DateTime(2026, 3, 11), BeerStyle = beerStyles[5] },

            new Keg { Id = 40, SLCode = "SL219", BestBefore = new DateTime(2026, 4, 28), Material = KegMaterial.Metal, HeadType = KegHeadType.KeyHead, VolumeInLitres = 30, SerialNumber = "KG-POR-001", LastInspection = new DateTime(2026, 3, 13), BeerStyle = beerStyles[6] },
            new Keg { Id = 41, SLCode = "SL220", BestBefore = new DateTime(2026, 6, 3), Material = KegMaterial.Plastic, HeadType = KegHeadType.SHead, VolumeInLitres = 20, SerialNumber = "KG-POR-002", LastInspection = new DateTime(2026, 3, 15), BeerStyle = beerStyles[6] },
            new Keg { Id = 42, SLCode = "SL221", BestBefore = new DateTime(2026, 7, 15), Material = KegMaterial.Metal, HeadType = KegHeadType.SHead, VolumeInLitres = 30, SerialNumber = "KG-POR-003", LastInspection = new DateTime(2026, 3, 17), BeerStyle = beerStyles[6] }
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
            },
            new WarehouseLocation
            {
                Id = 4,
                LocationCode = "D-04-1",
                Aisle = "D",
                Shelf = 1,
                MaxCapacity = 160,
                Description = "Mixed stock buffer near packing station"
            },
            new WarehouseLocation
            {
                Id = 5,
                LocationCode = "E-05-4",
                Aisle = "E",
                Shelf = 4,
                MaxCapacity = 200,
                Description = "High shelf for overstock cans"
            },
            new WarehouseLocation
            {
                Id = 6,
                LocationCode = "F-06-2",
                Aisle = "F",
                Shelf = 2,
                MaxCapacity = 140,
                Description = "Dispatch-prep lane for upcoming shipments"
            },
            new WarehouseLocation
            {
                Id = 7,
                LocationCode = "G-07-1",
                Aisle = "G",
                Shelf = 1,
                MaxCapacity = 60,
                Description = "Overflow quarantine zone"
            }
        };

        List<Container> containers = cans.Cast<Container>().Concat(kegs.Cast<Container>()).ToList();
        stockEntries = new List<StockEntry>();
        int stockEntryId = 1;

        for (int i = 0; i < containers.Count; i++)
        {
            int locationIndex = i % (warehouseLocations.Count - 1);
            if (i % 4 == 0)
            {
                locationIndex = 2;
            }

            WarehouseLocation location = warehouseLocations[locationIndex];
            DateTime receivedDate = new DateTime(2026, 3, 18).AddDays((i % 21) - 10);
            int quantity = containers[i] is Can ? 18 + ((i % 5) * 12) : 1 + (i % 6);
            string notes = i % 11 == 0
                ? string.Empty
                : containers[i] is Can
                    ? "Palletized can stock ready for retail orders"
                    : "Keg stock for taproom and wholesale dispatch";

            StockEntry stockEntry = new StockEntry
            {
                Id = stockEntryId++,
                Container = containers[i],
                Location = location,
                Quantity = quantity,
                DateReceived = receivedDate,
                DateModified = receivedDate.AddDays(2),
                Notes = notes
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
            },
            new Employee
            {
                Id = 4,
                FirstName = "Jonas",
                LastName = "Zielinski",
                Email = "jonas.zielinski@brewerywarehouse.local",
                Role = "Inventory Planner",
                DateHired = new DateTime(2020, 6, 17),
                IsActive = true
            },
            new Employee
            {
                Id = 5,
                FirstName = "Karolina",
                LastName = "Mroz",
                Email = "karolina.mroz@brewerywarehouse.local",
                Role = "Dispatch Coordinator",
                DateHired = new DateTime(2024, 2, 5),
                IsActive = true
            },
            new Employee
            {
                Id = 6,
                FirstName = "Piotr",
                LastName = "Lewandowski",
                Email = "piotr.lewandowski@brewerywarehouse.local",
                Role = "Warehouse Associate",
                DateHired = new DateTime(2023, 5, 29),
                IsActive = false
            },
            new Employee
            {
                Id = 7,
                FirstName = "Aneta",
                LastName = "Czarnecka",
                Email = "aneta.czarnecka@brewerywarehouse.local",
                Role = "Safety Officer",
                DateHired = new DateTime(2019, 9, 2),
                IsActive = true
            },
            new Employee
            {
                Id = 8,
                FirstName = "Milosz",
                LastName = "Brzezinski-Nowak",
                Email = "milosz.brzezinski-nowak@brewerywarehouse.local",
                Role = "Quality and Regulatory Documentation Specialist",
                DateHired = new DateTime(2026, 1, 10),
                IsActive = false
            },
            new Employee
            {
                Id = 9,
                FirstName = "Ola",
                LastName = "Jankowska",
                Email = "ola.jankowska@brewerywarehouse.local",
                Role = "Taproom Liaison",
                DateHired = new DateTime(2025, 12, 15),
                IsActive = true
            }
        };
    }
}
