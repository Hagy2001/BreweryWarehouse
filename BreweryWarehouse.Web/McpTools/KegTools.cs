using System.ComponentModel;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class KegTools
{
    private readonly KegRepository _repo;
    private readonly ILogger<KegTools> _logger;

    public KegTools(KegRepository repo, ILogger<KegTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_kegs")]
    [Description("Lists all keg containers in the warehouse inventory, excluding soft-deleted records. Each result includes the keg's SL code, material, head type, volume, serial number, last inspection date, best-before date, and associated beer style.")]
    public IEnumerable<KegDto> ListKegs()
    {
        _logger.LogInformation("MCP tool invoked: list_kegs");
        return _repo.GetAll().Select(k => k.ToDto());
    }

    [McpServerTool(Name = "get_keg")]
    [Description("Retrieves a single keg container by its unique integer ID, including its associated beer style. Returns null if the keg does not exist or has been soft-deleted.")]
    public KegDto? GetKeg(
        [Description("The unique integer ID of the keg to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_keg(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }
}
