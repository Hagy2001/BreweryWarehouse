namespace BreweryWarehouse.Model;

public class WarehouseLocation
{
    public int Id { get; set; }

    public string LocationCode { get; set; } = string.Empty;

    public string Aisle { get; set; } = string.Empty;

    public int Shelf { get; set; }

    public int MaxCapacity { get; set; }

    public string Description { get; set; } = string.Empty;

    public List<StockEntry> StockEntries { get; set; }

    public WarehouseLocation()
    {
        StockEntries = new List<StockEntry>();
    }
}
