namespace BreweryWarehouse.Web.Models.Dtos;

public class WarehouseLocationDto
{
    public int     Id           { get; set; }
    public string  LocationCode { get; set; } = string.Empty;
    public string  Aisle        { get; set; } = string.Empty;
    public int     Shelf        { get; set; }
    public int     MaxCapacity  { get; set; }
    public string? Description  { get; set; }
}
