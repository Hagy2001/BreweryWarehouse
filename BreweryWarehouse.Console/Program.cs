using BreweryWarehouse.Model;

await LoadWarehouseDataAsync();

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
DateTime cutoffDate = now.AddDays(180);

// Finds all seeded cans and kegs expiring within the next 180 days and orders them by earliest best-before date.
List<Container> expiringContainers = cans
	.Cast<Container>()
	.Concat(kegs.Cast<Container>())
	.Where(container => container.BestBefore >= now && container.BestBefore <= cutoffDate)
	.OrderBy(container => container.BestBefore)
	.ToList();

Console.WriteLine("Expiring in the next 180 days:");

foreach (Container container in expiringContainers)
{
	Console.WriteLine($"{container.SLCode} - {container.BestBefore:yyyy-MM-dd}");
}

string targetLocationCode = "A-01-3";

// Finds stock entries stored at a specific warehouse location by matching each entry's Location against a subquery filtered by LocationCode.
List<StockEntry> stockEntriesAtLocation = stockEntries
	.Where(entry => locations
		.Where(location => location.LocationCode == targetLocationCode)
		.Any(location => location.Id == entry.Location.Id))
	.ToList();

Console.WriteLine($"Stock entries at location {targetLocationCode}:");

foreach (StockEntry entry in stockEntriesAtLocation)
{
	string containerType = entry.Container is Can ? "Can" : "Keg";
	Console.WriteLine($"{entry.Container.SLCode} - {containerType} - Qty: {entry.Quantity}");
}

// Groups stock entries by the beer style name of each linked container, sums total quantity per style, and sorts from highest to lowest quantity.
var quantityByStyle = stockEntries
	.GroupBy(entry => entry.Container switch
	{
		Can can => can.BeerStyle.Name,
		Keg keg => keg.BeerStyle.Name,
		_ => "Unknown Style"
	})
	.Select(group => new
	{
		BeerStyleName = group.Key,
		TotalQuantity = group.Sum(entry => entry.Quantity)
	})
	.OrderByDescending(item => item.TotalQuantity)
	.ToList();

Console.WriteLine("Total quantity by beer style:");

foreach (var styleTotal in quantityByStyle)
{
	Console.WriteLine($"{styleTotal.BeerStyleName} - Qty: {styleTotal.TotalQuantity}");
}

string targetSlCode = "SL205";

// Combines cans and kegs into one sequence and finds exactly one container by SLCode, or null when no match is found.
Container? matchingContainer = cans
	.Cast<Container>()
	.Concat(kegs.Cast<Container>())
	.SingleOrDefault(container => container.SLCode == targetSlCode);

if (matchingContainer is null)
{
	Console.WriteLine("Not found");
}
else
{
	Console.WriteLine($"Found {matchingContainer.GetType().Name}: {matchingContainer.SLCode} - {matchingContainer.BestBefore:yyyy-MM-dd}");
}

static async Task LoadWarehouseDataAsync()
{
	Console.WriteLine("Loading warehouse data...");

	// await on Task.Delay pauses this async method without blocking the thread, while Thread.Sleep would block the current thread for the full duration.
	await Task.Delay(1500);

	Console.WriteLine("Warehouse data loaded.");
}
