using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models.Dtos;
using BreweryWarehouse.Web.Repositories;

namespace BreweryWarehouse.Web.Controllers.Api;

[Route("api/beer-styles")]
[ApiController]
public class BeerStyleApiController : ControllerBase
{
    private readonly BeerStyleRepository _repo;

    public BeerStyleApiController(BeerStyleRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<IEnumerable<BeerStyleDto>> GetAll([FromQuery] string? q)
    {
        var items = _repo.GetAll();
        if (!string.IsNullOrWhiteSpace(q))
            items = items.Where(b =>
                b.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                b.Description.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(items.Select(b => b.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public ActionResult<BeerStyleDto> GetById(int id)
    {
        var item = _repo.GetById(id);
        if (item == null) return NotFound();
        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<BeerStyleDto> Create([FromBody] BeerStyleRequestDto dto)
    {
        var entity = new BeerStyle
        {
            Name              = dto.Name,
            Description       = dto.Description,
            AlcoholPercentage = dto.AlcoholPercentage,
            IBU               = dto.IBU,
            ColorEBC          = dto.ColorEBC,
            Category          = dto.Category
        };
        _repo.Add(entity);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public ActionResult<BeerStyleDto> Update(int id, [FromBody] BeerStyleRequestDto dto)
    {
        var entity = _repo.GetById(id);
        if (entity == null) return NotFound();
        entity.Name              = dto.Name;
        entity.Description       = dto.Description;
        entity.AlcoholPercentage = dto.AlcoholPercentage;
        entity.IBU               = dto.IBU;
        entity.ColorEBC          = dto.ColorEBC;
        entity.Category          = dto.Category;
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
