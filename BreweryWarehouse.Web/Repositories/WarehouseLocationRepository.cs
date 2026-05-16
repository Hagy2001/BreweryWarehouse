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

    public void Add(WarehouseLocation location)
    {
        _context.WarehouseLocations.Add(location);
        _context.SaveChanges();
    }

    public void Update()
    {
        _context.SaveChanges();
    }

    public void Delete(WarehouseLocation location)
    {
        _context.WarehouseLocations.Remove(location);
        _context.SaveChanges();
    }
}
