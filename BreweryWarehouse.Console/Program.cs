using BreweryWarehouse.Model;

DataSeeder.Seed(
	out List<BeerStyle> beerStyles,
	out List<Can> cans,
	out List<Keg> kegs,
	out List<WarehouseLocation> locations,
	out List<StockEntry> stockEntries,
	out List<Employee> employees);

Console.WriteLine(
	$"Seeded {beerStyles.Count} beer styles, {cans.Count} cans, {kegs.Count} kegs, {locations.Count} locations, {stockEntries.Count} stock entries, {employees.Count} employees");
