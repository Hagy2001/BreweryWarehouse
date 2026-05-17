using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models;

public class CanEditModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "SL Code")]
    public string SLCode { get; set; }

    [Required]
    [Display(Name = "Best Before")]
    public DateTime? BestBefore { get; set; }

    [Required]
    [Display(Name = "Size")]
    public CanSize Size { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 4)]
    [Display(Name = "Barcode")]
    public string Barcode { get; set; }

    [Required]
    [Display(Name = "Packaging Date")]
    public DateTime? PackagingDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a beer style.")]
    [Display(Name = "Beer Style")]
    public int BeerStyleId { get; set; }

    public string? BeerStyleName { get; set; }
}
