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

DateTime now = DateTime.Now;
DateTime cutoffDate = now.AddDays(90);

// Finds all seeded cans and kegs expiring within the next 90 days and orders them by earliest best-before date.
List<Container> expiringContainers = cans
	.Cast<Container>()
	.Concat(kegs.Cast<Container>())
	.Where(container => container.BestBefore >= now && container.BestBefore <= cutoffDate)
	.OrderBy(container => container.BestBefore)
	.ToList();

Console.WriteLine("Expiring in the next 90 days:");

foreach (Container container in expiringContainers)
{
	Console.WriteLine($"{container.SLCode} - {container.BestBefore:yyyy-MM-dd}");
}
