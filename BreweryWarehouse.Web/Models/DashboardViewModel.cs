using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Models;

public class DashboardViewModel
{
    public int TotalCans { get; set; }

    public int TotalKegs { get; set; }

    public int TotalLocations { get; set; }

    public int TotalBeerStyles { get; set; }

    public List<Can> ExpiringCans { get; set; } = new();

    public List<Keg> ExpiringKegs { get; set; } = new();

    public List<WarehouseLocation> Locations { get; set; } = new();
}
