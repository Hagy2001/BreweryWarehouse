using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class CanRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public CanRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<Can> GetAll()
    {
        return _context.Cans
            .Include(can => can.BeerStyle)
            .Include(can => can.StockEntries)
            .ThenInclude(stockEntry => stockEntry.Location)
            .ToList();
    }

    public Can? GetById(int id)
    {
        return _context.Cans
            .Include(can => can.BeerStyle)
            .Include(can => can.StockEntries)
            .ThenInclude(stockEntry => stockEntry.Location)
            .FirstOrDefault(can => can.Id == id);
    }
}
