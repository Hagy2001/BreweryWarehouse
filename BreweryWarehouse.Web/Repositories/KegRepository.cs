using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class KegRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public KegRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<Keg> GetAll()
    {
        return _context.Kegs
            .Include(keg => keg.BeerStyle)
            .ToList();
    }

    public Keg? GetById(int id)
    {
        return _context.Kegs
            .Include(keg => keg.BeerStyle)
            .FirstOrDefault(keg => keg.Id == id);
    }
}
