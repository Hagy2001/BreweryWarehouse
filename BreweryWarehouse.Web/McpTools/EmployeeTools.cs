using System.ComponentModel;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;
using ModelContextProtocol.Server;

namespace BreweryWarehouse.Web.McpTools;

[McpServerToolType]
public class EmployeeTools
{
    private readonly EmployeeRepository _repo;
    private readonly ILogger<EmployeeTools> _logger;

    public EmployeeTools(EmployeeRepository repo, ILogger<EmployeeTools> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [McpServerTool(Name = "list_employees")]
    [Description("Lists all employees in the brewery warehouse system. Each result includes the employee's name, email, role, hire date, and active status.")]
    public IEnumerable<EmployeeDto> ListEmployees()
    {
        _logger.LogInformation("MCP tool invoked: list_employees");
        return _repo.GetAll().Select(e => e.ToDto());
    }

    [McpServerTool(Name = "get_employee")]
    [Description("Retrieves a single employee by their unique integer ID. Returns null if the employee does not exist.")]
    public EmployeeDto? GetEmployee(
        [Description("The unique integer ID of the employee to retrieve.")] int id)
    {
        _logger.LogInformation("MCP tool invoked: get_employee(id={Id})", id);
        var item = _repo.GetById(id);
        return item?.ToDto();
    }
}
