namespace BreweryWarehouse.Model;

public class BeerStyle
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double AlcoholPercentage { get; set; }

    public int IBU { get; set; }

    public double ColorEBC { get; set; }

    public BeerCategory Category { get; set; }

    public List<Can> Cans { get; set; }

    public List<Keg> Kegs { get; set; }

    public BeerStyle()
    {
        Cans = new List<Can>();
        Kegs = new List<Keg>();
    }
}
