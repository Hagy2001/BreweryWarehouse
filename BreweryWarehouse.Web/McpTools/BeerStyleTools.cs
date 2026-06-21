using System.ComponentModel;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class BeerStyleTools
{
    private readonly BeerStyleRepository _repo;
    private readonly ILogger<BeerStyleTools> _logger;

    public BeerStyleTools(BeerStyleRepository repo, ILogger<BeerStyleTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_beer_styles")]
    [Description("Lists all beer styles in the warehouse inventory, excluding soft-deleted records. Returns id, name, description, alcohol percentage, IBU, color EBC, and category for each style.")]
    public IEnumerable<BeerStyleDto> ListBeerStyles()
    {
        _logger.LogInformation("MCP tool invoked: list_beer_styles");
        return _repo.GetAll().Select(b => b.ToDto());
    }

    [McpServerTool(Name = "get_beer_style")]
    [Description("Retrieves a single beer style by its unique integer ID. Returns null if the style does not exist or has been soft-deleted.")]
    public BeerStyleDto? GetBeerStyle(
        [Description("The unique integer ID of the beer style to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_beer_style(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }

    [McpServerTool(Name = "create_beer_style")]
    [Description("Creates a new beer style and persists it to the warehouse database. Returns the newly created beer style including its assigned ID.")]
    public BeerStyleDto CreateBeerStyle(
        [Description("Beer style name (2–100 characters).")] string name,
        [Description("Detailed description of the beer style (up to 500 characters).")] string description,
        [Description("Alcohol by volume percentage (0.0–100.0).")] double alcoholPercentage,
        [Description("International Bitterness Units (0–1000).")] int ibu,
        [Description("Beer color on the EBC scale (0.0–1000.0).")] double colorEbc,
        [Description("Beer category — must be one of: Lager, Ale, IPA, Stout, Wheat, Sour, Porter.")] string category)
    {
        if (!Enum.TryParse<BeerCategory>(category, ignoreCase: true, out var parsedCategory))
            throw new ArgumentException($"Invalid category '{category}'. Valid values: {string.Join(", ", Enum.GetNames<BeerCategory>())}");

        var entity = new BeerStyle
        {
            Name              = name,
            Description       = description,
            AlcoholPercentage = alcoholPercentage,
            IBU               = ibu,
            ColorEBC          = colorEbc,
            Category          = parsedCategory
        };
        _repo.Add(entity);

        _logger.LogInformation("MCP tool invoked: create_beer_style — created Id={Id}, Name={Name}", entity.Id, entity.Name);
        return entity.ToDto();
    }
}
