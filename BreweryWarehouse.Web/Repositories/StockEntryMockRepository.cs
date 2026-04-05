using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class StockEntryMockRepository
{
    private static readonly List<StockEntry> StockEntries;

    static StockEntryMockRepository()
    {
        DataSeeder.Seed(
            out _,
            out _,
            out _,
            out _,
            out List<StockEntry> stockEntries,
            out _);

        StockEntries = stockEntries;
    }

    public List<StockEntry> GetAll()
    {
        return StockEntries;
    }

    public StockEntry? GetById(int id)
    {
        return StockEntries.FirstOrDefault(entry => entry.Id == id);
    }
}
