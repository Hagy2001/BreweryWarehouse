using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models.Dtos;

public class BeerStyleRequestDto
{
    [Required][StringLength(100, MinimumLength = 2)]
    public string Name              { get; set; } = string.Empty;

    [Required][StringLength(500)]
    public string Description       { get; set; } = string.Empty;

    [Required][Range(0.0, 100.0)]
    public double AlcoholPercentage { get; set; }

    [Required][Range(0, 1000)]
    public int IBU                  { get; set; }

    [Required][Range(0.0, 1000.0)]
    public double ColorEBC          { get; set; }

    [Required]
    public BeerCategory Category    { get; set; }
}
