using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class KegMockRepository
{
    private static readonly List<Keg> Kegs;

    static KegMockRepository()
    {
        DataSeeder.Seed(
            out _,
            out _,
            out List<Keg> kegs,
            out _,
            out _,
            out _);

        Kegs = kegs;
    }

    public List<Keg> GetAll()
    {
        return Kegs;
    }

    public Keg? GetById(int id)
    {
        return Kegs.FirstOrDefault(keg => keg.Id == id);
    }
}
