using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class StockEntryRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public StockEntryRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<StockEntry> GetAll()
    {
        return _context.StockEntries
            .Include(stockEntry => stockEntry.Container)
            .Include(stockEntry => stockEntry.Location)
            .Include("Container.BeerStyle")
            .ToList();
    }

    public StockEntry? GetById(int id)
    {
        return _context.StockEntries
            .Include(stockEntry => stockEntry.Container)
            .Include(stockEntry => stockEntry.Location)
            .Include("Container.BeerStyle")
            .FirstOrDefault(stockEntry => stockEntry.Id == id);
    }

    public void Add(StockEntry stockEntry)
    {
        _context.StockEntries.Add(stockEntry);
        _context.SaveChanges();
    }

    public void Update()
    {
        _context.SaveChanges();
    }

    public void Delete(StockEntry stockEntry)
    {
        _context.StockEntries.Remove(stockEntry);
        _context.SaveChanges();
    }
}
