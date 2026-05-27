using System.ComponentModel.DataAnnotations;
using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models;

public class BeerStyleCreateModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 5)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.0, 100.0, ErrorMessage = "Alcohol percentage must be between 0 and 100.")]
    [Display(Name = "Alcohol %")]
    public double AlcoholPercentage { get; set; }

    [Required]
    [Range(0, 120, ErrorMessage = "IBU must be between 0 and 120.")]
    [Display(Name = "IBU")]
    public int IBU { get; set; }

    [Required]
    [Range(0.0, 80.0, ErrorMessage = "Color EBC must be between 0 and 80.")]
    [Display(Name = "Color (EBC)")]
    public double ColorEBC { get; set; }

    [Required]
    [Display(Name = "Category")]
    public BeerCategory Category { get; set; }
}
