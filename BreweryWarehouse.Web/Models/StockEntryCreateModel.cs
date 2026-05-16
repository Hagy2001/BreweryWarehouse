using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models;

public class StockEntryCreateModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a container.")]
    [Display(Name = "Container")]
    public int ContainerId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a location.")]
    [Display(Name = "Location")]
    public int LocationId { get; set; }

    [Required]
    [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000.")]
    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    [Required]
    [Display(Name = "Date Received")]
    public DateTime DateReceived { get; set; }

    [StringLength(1000)]
    [Display(Name = "Notes")]
    public string Notes { get; set; }
}
