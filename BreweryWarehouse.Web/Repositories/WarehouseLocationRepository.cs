using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class WarehouseLocationRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public WarehouseLocationRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<WarehouseLocation> GetAll()
    {
        return _context.WarehouseLocations
            .Include(location => location.StockEntries)
            .ThenInclude(stockEntry => stockEntry.Container)
            .Include("StockEntries.Container.BeerStyle")
            .ToList();
    }

    public WarehouseLocation? GetById(int id)
    {
        return _context.WarehouseLocations
            .Include(location => location.StockEntries)
            .ThenInclude(stockEntry => stockEntry.Container)
            .Include("StockEntries.Container.BeerStyle")
            .FirstOrDefault(location => location.Id == id);
    }
}
