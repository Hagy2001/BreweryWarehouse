using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/kegs")]
[ApiController]
public class KegApiController : ControllerBase
{
    private readonly KegRepository _repo;

    public KegApiController(KegRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<KegDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(k =>
                k.SLCode.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                k.SerialNumber.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(k => k.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<KegDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<KegDto> Create([FromBody] KegRequestDto dto)
    {
        var entity = new Keg
        {
            SLCode         = dto.SLCode,
            BestBefore     = dto.BestBefore,
            Material       = dto.Material,
            HeadType       = dto.HeadType,
            VolumeInLitres = dto.VolumeInLitres,
            SerialNumber   = dto.SerialNumber,
            LastInspection = dto.LastInspection,
            BeerStyleId    = dto.BeerStyleId
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<KegDto> Update(int id, [FromBody] KegRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.SLCode         = dto.SLCode;
        entity.BestBefore     = dto.BestBefore;
        entity.Material       = dto.Material;
        entity.HeadType       = dto.HeadType;
        entity.VolumeInLitres = dto.VolumeInLitres;
        entity.SerialNumber   = dto.SerialNumber;
        entity.LastInspection = dto.LastInspection;
        entity.BeerStyleId    = dto.BeerStyleId;
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
