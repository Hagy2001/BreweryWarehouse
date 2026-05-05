using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(BreweryWarehouseDbContext context)
    {
        if (context.BeerStyles.Any())
            return;

        DataSeeder.Seed(
            out List<BeerStyle> beerStyles,
            out List<Can> cans,
            out List<Keg> kegs,
            out List<WarehouseLocation> warehouseLocations,
            out List<StockEntry> stockEntries,
            out List<Employee> employees);

        foreach (var beerStyle in beerStyles) beerStyle.Id = 0;
        foreach (var location in warehouseLocations) location.Id = 0;
        foreach (var can in cans) can.Id = 0;
        foreach (var keg in kegs) keg.Id = 0;
        foreach (var stockEntry in stockEntries) stockEntry.Id = 0;
        foreach (var employee in employees) employee.Id = 0;

        context.BeerStyles.AddRange(beerStyles);
        await context.SaveChangesAsync();

        Dictionary<string, BeerStyle> beerStyleByName = beerStyles.ToDictionary(b => b.Name);
        Dictionary<string, WarehouseLocation> locationByCode = warehouseLocations.ToDictionary(l => l.LocationCode);
        Dictionary<string, Can> canBySlCode = cans.ToDictionary(c => c.SLCode);
        Dictionary<string, Keg> kegBySlCode = kegs.ToDictionary(k => k.SLCode);

        foreach (var can in cans)
        {
            if (can.BeerStyle is not null && beerStyleByName.TryGetValue(can.BeerStyle.Name, out var beerStyle))
                can.BeerStyle = beerStyle;
        }

        foreach (var keg in kegs)
        {
            if (keg.BeerStyle is not null && beerStyleByName.TryGetValue(keg.BeerStyle.Name, out var beerStyle))
                keg.BeerStyle = beerStyle;
        }

        context.WarehouseLocations.AddRange(warehouseLocations);
        await context.SaveChangesAsync();

        context.Cans.AddRange(cans);
        context.Kegs.AddRange(kegs);
        await context.SaveChangesAsync();

        foreach (var stockEntry in stockEntries)
        {
            if (stockEntry.Container is Can can && canBySlCode.TryGetValue(can.SLCode, out var matchedCan))
                stockEntry.Container = matchedCan;
            else if (stockEntry.Container is Keg keg && kegBySlCode.TryGetValue(keg.SLCode, out var matchedKeg))
                stockEntry.Container = matchedKeg;

            if (stockEntry.Location is not null && locationByCode.TryGetValue(stockEntry.Location.LocationCode, out var matchedLocation))
                stockEntry.Location = matchedLocation;
        }

        context.StockEntries.AddRange(stockEntries);
        await context.SaveChangesAsync();

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }
}