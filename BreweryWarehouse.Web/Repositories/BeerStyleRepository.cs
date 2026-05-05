using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Repositories;

public class BeerStyleRepository
{
    private readonly BreweryWarehouseDbContext _context;

    public BeerStyleRepository(BreweryWarehouseDbContext context)
    {
        _context = context;
    }

    public List<BeerStyle> GetAll()
    {
        return _context.BeerStyles.ToList();
    }

    public BeerStyle? GetById(int id)
    {
        return _context.BeerStyles.FirstOrDefault(beerStyle => beerStyle.Id == id);
    }
}
