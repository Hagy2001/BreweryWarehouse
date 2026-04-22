namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WarehouseLocation
{
    [Key]
    public int Id { get; set; }

    public string LocationCode { get; set; } = string.Empty;

    public string Aisle { get; set; } = string.Empty;

    public int Shelf { get; set; }

    public int MaxCapacity { get; set; }

    public string Description { get; set; } = string.Empty;

    public virtual ICollection<StockEntry> StockEntries { get; set; }

    public WarehouseLocation()
    {
        StockEntries = new List<StockEntry>();
    }
}
