using System.ComponentModel;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class StockEntryTools
{
    private readonly StockEntryRepository _repo;
    private readonly ILogger<StockEntryTools> _logger;

    public StockEntryTools(StockEntryRepository repo, ILogger<StockEntryTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_stock_entries")]
    [Description("Lists all stock entries in the warehouse. Each result includes the container summary (SL code, type, beer style), warehouse location, quantity on hand, date received, date last modified, and any notes.")]
    public IEnumerable<StockEntryDto> ListStockEntries()
    {
        _logger.LogInformation("MCP tool invoked: list_stock_entries");
        return _repo.GetAll().Select(s => s.ToDto());
    }

    [McpServerTool(Name = "get_stock_entry")]
    [Description("Retrieves a single stock entry by its unique integer ID, including its container and warehouse location details. Returns null if the entry does not exist.")]
    public StockEntryDto? GetStockEntry(
        [Description("The unique integer ID of the stock entry to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_stock_entry(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }
}
