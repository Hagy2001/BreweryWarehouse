using System.ComponentModel;

namespace BreweryWarehouse.Model;

public enum BeerCategory
{
    [Description("Lager")] Lager,
    [Description("Ale")] Ale,
    [Description("IPA")] IPA,
    [Description("Stout")] Stout,
    [Description("Wheat")] Wheat,
    [Description("Sour")] Sour,
    [Description("Porter")] Porter
}
