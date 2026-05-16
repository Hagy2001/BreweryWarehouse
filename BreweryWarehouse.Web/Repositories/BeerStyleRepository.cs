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
        return _context.BeerStyles
            .Where(beerStyle => beerStyle.DeletedAt == null)
            .Include(beerStyle => beerStyle.Cans)
            .Include(beerStyle => beerStyle.Kegs)
            .ToList();
    }

    public BeerStyle? GetById(int id)
    {
        return _context.BeerStyles
            .Include(beerStyle => beerStyle.Cans)
            .Include(beerStyle => beerStyle.Kegs)
            .FirstOrDefault(beerStyle => beerStyle.Id == id);
    }

    public void Add(BeerStyle beerStyle)
    {
        _context.BeerStyles.Add(beerStyle);
        _context.SaveChanges();
    }

    public void Update()
    {
        _context.SaveChanges();
    }

    public void SoftDelete(BeerStyle beerStyle)
    {
        beerStyle.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }
}
