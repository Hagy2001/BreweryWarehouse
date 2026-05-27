using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models.Dtos;

public class CanRequestDto
{
    [Required][StringLength(50, MinimumLength = 2)]
    public string   SLCode        { get; set; } = string.Empty;

    [Required]
    public DateTime BestBefore    { get; set; }

    [Required]
    public CanSize  Size          { get; set; }

    [Required][StringLength(50)]
    public string   Barcode       { get; set; } = string.Empty;

    [Required]
    public DateTime PackagingDate { get; set; }

    [Required][Range(1, int.MaxValue, ErrorMessage = "A valid BeerStyle must be selected.")]
    public int      BeerStyleId   { get; set; }
}
