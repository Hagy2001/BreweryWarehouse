using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class WarehouseLocationMockRepository
{
    private static readonly List<WarehouseLocation> WarehouseLocations;

    static WarehouseLocationMockRepository()
    {
        DataSeeder.Seed(
            out _,
            out _,
            out _,
            out List<WarehouseLocation> warehouseLocations,
            out _,
            out _);

        WarehouseLocations = warehouseLocations;
    }

    public List<WarehouseLocation> GetAll()
    {
        return WarehouseLocations;
    }

    public WarehouseLocation? GetById(int id)
    {
        return WarehouseLocations.FirstOrDefault(location => location.Id == id);
    }
}
