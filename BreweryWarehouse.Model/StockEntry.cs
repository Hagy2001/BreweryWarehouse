namespace BreweryWarehouse.Model;

public class StockEntry
{
	public int Id { get; set; }

	public Container Container { get; set; } = null!;

	public WarehouseLocation Location { get; set; } = null!;

	public int Quantity { get; set; }

	public DateTime DateReceived { get; set; }

	public DateTime DateModified { get; set; }

	public string Notes { get; set; } = string.Empty;
}
