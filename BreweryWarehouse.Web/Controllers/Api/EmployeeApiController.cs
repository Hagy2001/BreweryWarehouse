using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/employees")]
[ApiController]
public class EmployeeApiController : ControllerBase
{
    private readonly EmployeeRepository _repo;

    public EmployeeApiController(EmployeeRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<EmployeeDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(e =>
                e.FirstName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                e.LastName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                e.Email.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(e => e.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<EmployeeDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<EmployeeDto> Create([FromBody] EmployeeRequestDto dto)
    {
        var entity = new Employee
        {
            FirstName = dto.FirstName,
            LastName  = dto.LastName,
            Email     = dto.Email,
            Role      = dto.Role,
            DateHired = dto.DateHired,
            IsActive  = dto.IsActive
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<EmployeeDto> Update(int id, [FromBody] EmployeeRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.FirstName = dto.FirstName;
        entity.LastName  = dto.LastName;
        entity.Email     = dto.Email;
        entity.Role      = dto.Role;
        entity.DateHired = dto.DateHired;
        entity.IsActive  = dto.IsActive;
        _repo.Update();
        return Ok(entity.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        _repo.Delete(entity);
        return NoContent();
    }
}
