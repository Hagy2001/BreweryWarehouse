using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models.Dtos;

public class KegRequestDto
{
    [Required][StringLength(50, MinimumLength = 2)]
    public string      SLCode         { get; set; } = string.Empty;

    [Required]
    public DateTime    BestBefore     { get; set; }

    [Required]
    public KegMaterial Material       { get; set; }

    [Required]
    public KegHeadType HeadType       { get; set; }

    [Required][Range(20, 30)]
    public int         VolumeInLitres { get; set; }

    [Required][StringLength(50)]
    public string      SerialNumber   { get; set; } = string.Empty;

    [Required]
    public DateTime    LastInspection { get; set; }

    [Required][Range(1, int.MaxValue, ErrorMessage = "A valid BeerStyle must be selected.")]
    public int         BeerStyleId    { get; set; }
}
