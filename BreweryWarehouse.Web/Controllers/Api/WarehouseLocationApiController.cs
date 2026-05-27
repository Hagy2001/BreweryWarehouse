using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/locations")]
[ApiController]
public class WarehouseLocationApiController : ControllerBase
{
    private readonly WarehouseLocationRepository _repo;

    public WarehouseLocationApiController(WarehouseLocationRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<WarehouseLocationDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(w =>
                w.LocationCode.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                w.Aisle.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(w => w.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<WarehouseLocationDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<WarehouseLocationDto> Create([FromBody] WarehouseLocationRequestDto dto)
    {
        var entity = new WarehouseLocation
        {
            LocationCode = dto.LocationCode,
            Aisle        = dto.Aisle,
            Shelf        = dto.Shelf,
            MaxCapacity  = dto.MaxCapacity,
            Description  = dto.Description ?? string.Empty
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<WarehouseLocationDto> Update(int id, [FromBody] WarehouseLocationRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.LocationCode = dto.LocationCode;
        entity.Aisle        = dto.Aisle;
        entity.Shelf        = dto.Shelf;
        entity.MaxCapacity  = dto.MaxCapacity;
        entity.Description  = dto.Description ?? string.Empty;
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
