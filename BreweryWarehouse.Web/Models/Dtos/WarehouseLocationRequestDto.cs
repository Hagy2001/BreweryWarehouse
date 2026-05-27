using System.ComponentModel.DataAnnotations;

namespace BreweryWarehouse.Web.Models.Dtos;

public class WarehouseLocationRequestDto
{
    [Required][StringLength(20, MinimumLength = 1)]
    public string  LocationCode { get; set; } = string.Empty;

    [Required][StringLength(20, MinimumLength = 1)]
    public string  Aisle        { get; set; } = string.Empty;

    [Required][Range(1, 100)]
    public int     Shelf        { get; set; }

    [Required][Range(1, 10000)]
    public int     MaxCapacity  { get; set; }

    [StringLength(300)]
    public string? Description  { get; set; }
}
