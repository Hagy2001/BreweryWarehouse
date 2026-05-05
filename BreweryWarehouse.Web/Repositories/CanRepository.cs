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
            .ToList();
    }

    public Can? GetById(int id)
    {
        return _context.Cans
            .Include(can => can.BeerStyle)
            .FirstOrDefault(can => can.Id == id);
    }
}
