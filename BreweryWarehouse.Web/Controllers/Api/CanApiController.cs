using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/cans")]
[ApiController]
public class CanApiController : ControllerBase
{
    private readonly CanRepository _repo;

    public CanApiController(CanRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<CanDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(c =>
                c.SLCode.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                c.Barcode.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(c => c.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<CanDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<CanDto> Create([FromBody] CanRequestDto dto)
    {
        var entity = new Can
        {
            SLCode        = dto.SLCode,
            BestBefore    = dto.BestBefore,
            Size          = dto.Size,
            Barcode       = dto.Barcode,
            PackagingDate = dto.PackagingDate,
            BeerStyleId   = dto.BeerStyleId
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<CanDto> Update(int id, [FromBody] CanRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.SLCode        = dto.SLCode;
        entity.BestBefore    = dto.BestBefore;
        entity.Size          = dto.Size;
        entity.Barcode       = dto.Barcode;
        entity.PackagingDate = dto.PackagingDate;
        entity.BeerStyleId   = dto.BeerStyleId;
        _repo.Update();
        return Ok(entity.ToDto());
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        _repo.SoftDelete(entity);
        return NoContent();
    }
}
