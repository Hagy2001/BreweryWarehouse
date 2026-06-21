using System.ComponentModel;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class WarehouseLocationTools
{
    private readonly WarehouseLocationRepository _repo;
    private readonly ILogger<WarehouseLocationTools> _logger;

    public WarehouseLocationTools(WarehouseLocationRepository repo, ILogger<WarehouseLocationTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_warehouse_locations")]
    [Description("Lists all warehouse storage locations. Each result includes the location code, aisle, shelf number, maximum capacity, and description.")]
    public IEnumerable<WarehouseLocationDto> ListWarehouseLocations()
    {
        _logger.LogInformation("MCP tool invoked: list_warehouse_locations");
        return _repo.GetAll().Select(w => w.ToDto());
    }

    [McpServerTool(Name = "get_warehouse_location")]
    [Description("Retrieves a single warehouse storage location by its unique integer ID. Returns null if the location does not exist.")]
    public WarehouseLocationDto? GetWarehouseLocation(
        [Description("The unique integer ID of the warehouse location to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_warehouse_location(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }
}
