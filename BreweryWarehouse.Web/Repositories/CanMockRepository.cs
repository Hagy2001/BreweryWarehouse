using BreweryWarehouse.Model;

namespace BreweryWarehouse.Web.Repositories;

public class CanMockRepository
{
    private static readonly List<Can> Cans;

    static CanMockRepository()
    {
        DataSeeder.Seed(
            out _,
            out List<Can> cans,
            out _,
            out _,
            out _,
            out _);

        Cans = cans;
    }

    public List<Can> GetAll()
    {
        return Cans;
    }

    public Can? GetById(int id)
    {
        return Cans.FirstOrDefault(can => can.Id == id);
    }
}
