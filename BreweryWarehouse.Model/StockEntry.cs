namespace BreweryWarehouse.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StockEntry
{
	[Key]
	public int Id { get; set; }

	public int ContainerId { get; set; }

	[ForeignKey("Container")]
	public virtual Container Container { get; set; } = null!;

	public int LocationId { get; set; }

	[ForeignKey("Location")]
	public virtual WarehouseLocation Location { get; set; } = null!;

	public int Quantity { get; set; }

	public DateTime DateReceived { get; set; }

	public DateTime DateModified { get; set; }

	public string Notes { get; set; } = string.Empty;
}
