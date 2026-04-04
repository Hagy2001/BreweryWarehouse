using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class BeerStyleMockRepository
{
    private static readonly List<BeerStyle> BeerStyles;

    static BeerStyleMockRepository()
    {
        DataSeeder.Seed(
            out List<BeerStyle> beerStyles,
            out _,
            out _,
            out _,
            out _,
            out _);

        BeerStyles = beerStyles;
    }

    public List<BeerStyle> GetAll()
    {
        return BeerStyles;
    }

    public BeerStyle? GetById(int id)
    {
        return BeerStyles.FirstOrDefault(beerStyle => beerStyle.Id == id);
    }
}
