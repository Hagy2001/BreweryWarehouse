using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models;

public class WarehouseLocationEditModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 1)]
    [Display(Name = "Location Code")]
    public string LocationCode { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 1)]
    [Display(Name = "Aisle")]
    public string Aisle { get; set; }

    [Required]
    [Range(1, 99, ErrorMessage = "Shelf must be between 1 and 99.")]
    [Display(Name = "Shelf")]
    public int Shelf { get; set; }

    [Required]
    [Range(1, 10000, ErrorMessage = "Max capacity must be between 1 and 10000.")]
    [Display(Name = "Max Capacity")]
    public int MaxCapacity { get; set; }

    [StringLength(500)]
    [Display(Name = "Description")]
    public string Description { get; set; }
}
