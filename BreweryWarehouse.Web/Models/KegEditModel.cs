using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models;

public class KegEditModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "SL Code")]
    public required string SLCode { get; set; }

    [Required]
    [Display(Name = "Best Before")]
    public DateTime? BestBefore { get; set; }

    [Required]
    [Display(Name = "Material")]
    public KegMaterial Material { get; set; }

    [Required]
    [Display(Name = "Head Type")]
    public KegHeadType HeadType { get; set; }

    [Required]
    [Range(20, 30, ErrorMessage = "Volume must be 20 or 30 litres.")]
    [Display(Name = "Volume (L)")]
    public int VolumeInLitres { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Serial Number")]
    public required string SerialNumber { get; set; }

    [Required]
    [Display(Name = "Last Inspection")]
    public DateTime? LastInspection { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a beer style.")]
    [Display(Name = "Beer Style")]
    public int BeerStyleId { get; set; }

    public string? BeerStyleName { get; set; }
}
