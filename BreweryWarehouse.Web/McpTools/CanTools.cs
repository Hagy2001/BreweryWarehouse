using System.ComponentModel;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class CanTools
{
    private readonly CanRepository _repo;
    private readonly ILogger<CanTools> _logger;

    public CanTools(CanRepository repo, ILogger<CanTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_cans")]
    [Description("Lists all canned beer containers in the warehouse inventory, excluding soft-deleted records. Each result includes the can's SL code, size, barcode, packaging date, best-before date, and associated beer style.")]
    public IEnumerable<CanDto> ListCans()
    {
        _logger.LogInformation("MCP tool invoked: list_cans");
        return _repo.GetAll().Select(c => c.ToDto());
    }

    [McpServerTool(Name = "get_can")]
    [Description("Retrieves a single canned beer container by its unique integer ID, including its associated beer style. Returns null if the can does not exist or has been soft-deleted.")]
    public CanDto? GetCan(
        [Description("The unique integer ID of the can to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_can(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }
}
