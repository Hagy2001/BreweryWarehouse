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
            .ToList();
    }

    public StockEntry? GetById(int id)
    {
        return _context.StockEntries
            .Include(stockEntry => stockEntry.Container)
            .Include(stockEntry => stockEntry.Location)
            .FirstOrDefault(stockEntry => stockEntry.Id == id);
    }
}
