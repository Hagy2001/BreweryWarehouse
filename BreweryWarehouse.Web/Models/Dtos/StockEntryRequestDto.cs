using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models.Dtos;

public class StockEntryRequestDto
{
    [Required][Range(1, int.MaxValue, ErrorMessage = "A valid Container must be selected.")]
    public int     ContainerId  { get; set; }

    [Required][Range(1, int.MaxValue, ErrorMessage = "A valid Location must be selected.")]
    public int     LocationId   { get; set; }

    [Required][Range(1, 100000)]
    public int     Quantity     { get; set; }

    [Required]
    public DateTime DateReceived { get; set; }

    [StringLength(500)]
    public string?  Notes        { get; set; }
}
