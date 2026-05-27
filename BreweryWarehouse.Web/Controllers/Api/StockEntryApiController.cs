using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/stock-entries")]
[ApiController]
public class StockEntryApiController : ControllerBase
{
    private readonly StockEntryRepository _repo;

    public StockEntryApiController(StockEntryRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<StockEntryDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(s =>
                !string.IsNullOrWhiteSpace(s.Notes) &&
                s.Notes.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(s => s.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<StockEntryDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<StockEntryDto> Create([FromBody] StockEntryRequestDto dto)
    {
        var entity = new StockEntry
        {
            ContainerId  = dto.ContainerId,
            LocationId   = dto.LocationId,
            Quantity     = dto.Quantity,
            DateReceived = dto.DateReceived,
            DateModified = DateTime.UtcNow,
            Notes        = dto.Notes ?? string.Empty
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<StockEntryDto> Update(int id, [FromBody] StockEntryRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.ContainerId  = dto.ContainerId;
        entity.LocationId   = dto.LocationId;
        entity.Quantity     = dto.Quantity;
        entity.DateReceived = dto.DateReceived;
        entity.DateModified = DateTime.UtcNow;
        entity.Notes        = dto.Notes ?? string.Empty;
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
