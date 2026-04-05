namespace BreweryWarehouse.Model;

public class Can : Container
{
	public CanSize Size { get; set; }

	public string Barcode { get; set; } = string.Empty;

	public DateTime PackagingDate { get; set; }

	public List<StockEntry> StockEntries { get; set; }

	public BeerStyle BeerStyle { get; set; } = null!;

	public Can()
	{
		StockEntries = new List<StockEntry>();
	}

	
}
