namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Can : Container
{
	public CanSize Size { get; set; }

	public string Barcode { get; set; } = string.Empty;

	public DateTime PackagingDate { get; set; }

	public virtual ICollection<StockEntry> StockEntries { get; set; }

	public int BeerStyleId { get; set; }

	[ForeignKey(nameof(BeerStyleId))]
	public virtual BeerStyle BeerStyle { get; set; } = null!;

	public Can()
	{
		StockEntries = new List<StockEntry>();
	}

	
}
