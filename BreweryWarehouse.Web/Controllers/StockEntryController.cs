using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BreweryWarehouse.Web.Controllers;

[Authorize]
public class StockEntryController : Controller
{
    private readonly BreweryWarehouseDbContext _context;
    private readonly StockEntryRepository repository;
    private readonly CanRepository canRepository;
    private readonly KegRepository kegRepository;
    private readonly WarehouseLocationRepository locationRepository;

    public StockEntryController(
        BreweryWarehouseDbContext context,
        StockEntryRepository repository,
        CanRepository canRepository,
        KegRepository kegRepository,
        WarehouseLocationRepository locationRepository)
    {
        _context = context;
        this.repository = repository;
        this.canRepository = canRepository;
        this.kegRepository = kegRepository;
        this.locationRepository = locationRepository;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [HttpGet]
    [Route("StockEntry/search")]
    [AllowAnonymous]
    public JsonResult Search(string? q)
    {
        IEnumerable<StockEntry> entries = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            entries = entries.Where(entry =>
                (entry.Container.SLCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (entry.Location.LocationCode?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var results = entries
            .OrderBy(entry => entry.Id)
            .Take(20)
            .Select(entry =>
            {
                bool isCan = entry.Container is Can;
                string beerStyleName = "-";

                if (isCan && entry.Container is Can can && can.BeerStyle is not null)
                {
                    beerStyleName = can.BeerStyle.Name;
                }
                else if (!isCan && entry.Container is Keg keg && keg.BeerStyle is not null)
                {
                    beerStyleName = keg.BeerStyle.Name;
                }

                return new
                {
                    entry.Id,
                    ContainerSLCode = entry.Container.SLCode,
                    ContainerType = isCan ? "CAN" : "KEG",
                    BeerStyleName = beerStyleName,
                    LocationCode = entry.Location.LocationCode,
                    entry.Quantity,
                    DateReceivedDisplay = entry.DateReceived.ToString("dd MMM yyyy")
                };
            })
            .ToList();

        return Json(results);
    }

    [Authorize]
    public IActionResult Details(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        ViewBag.Attachments = _context.Attachments
            .Where(a => a.StockEntryId == id)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();

        return View(stockEntry);
    }

    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View(new StockEntryCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create(StockEntryCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(model);
        }

        StockEntry stockEntry = new StockEntry
        {
            ContainerId = model.ContainerId,
            LocationId = model.LocationId,
            Quantity = model.Quantity,
            DateReceived = model.DateReceived!.Value,
            DateModified = DateTime.UtcNow,
            Notes = model.Notes ?? string.Empty
        };

        repository.Add(stockEntry);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        StockEntryEditModel model = new StockEntryEditModel
        {
            Id = stockEntry.Id,
            ContainerId = stockEntry.ContainerId,
            LocationId = stockEntry.LocationId,
            Quantity = stockEntry.Quantity,
            DateReceived = stockEntry.DateReceived,
            Notes = stockEntry.Notes
        };

        PopulateDropdowns();
        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id, StockEntryEditModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(model);
        }

        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        stockEntry.ContainerId = model.ContainerId;
        stockEntry.LocationId = model.LocationId;
        stockEntry.Quantity = model.Quantity;
        stockEntry.DateReceived = model.DateReceived!.Value;
        stockEntry.DateModified = DateTime.UtcNow;
        stockEntry.Notes = model.Notes ?? string.Empty;

        repository.Update();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        StockEntry? stockEntry = repository.GetById(id);

        if (stockEntry is null)
        {
            return NotFound();
        }

        repository.Delete(stockEntry);

        return RedirectToAction("Index");
    }

    private void PopulateDropdowns()
    {
        var containers = canRepository.GetAll()
            .Cast<Container>()
            .Concat(kegRepository.GetAll())
            .OrderBy(container => container.SLCode)
            .Select(container => new
            {
                container.Id,
                Label = $"{container.SLCode} ({container.GetType().Name})"
            })
            .ToList();

        var locations = locationRepository.GetAll()
            .OrderBy(location => location.LocationCode)
            .Select(location => new
            {
                location.Id,
                Label = $"{location.LocationCode} (Aisle {location.Aisle}, Shelf {location.Shelf})"
            })
            .ToList();

        ViewBag.Containers = new SelectList(containers, "Id", "Label");
        ViewBag.Locations = new SelectList(locations, "Id", "Label");
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    [Route("stock-entries/{id}/upload")]
    public async Task<IActionResult> UploadAttachment(int id, IFormFile file)
    {
        var entry = await _context.StockEntries
            .FirstOrDefaultAsync(s => s.Id == id);

        if (entry == null) return NotFound();

        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot", "uploads", "stock-entries", id.ToString());

        Directory.CreateDirectory(uploadsPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);
        var publicUrl = $"/uploads/stock-entries/{id}/{fileName}";

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            StockEntryId = id,
            FileName = file.FileName,
            FilePath = publicUrl,
            ContentType = file.ContentType,
            FileSize = file.Length,
            CreatedAt = DateTime.UtcNow
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    [HttpGet]
    [Authorize]
    [Route("stock-entries/{id}/attachments")]
    public IActionResult GetAttachments(int id)
    {
        var attachments = _context.Attachments
            .Where(a => a.StockEntryId == id)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();

        return PartialView("_AttachmentList", attachments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,WarehouseManager")]
    [Route("stock-entries/attachments/{attachmentId}/delete")]
    public async Task<IActionResult> DeleteAttachment(int attachmentId)
    {
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(a => a.Id == attachmentId);

        if (attachment == null) return NotFound();

        var physicalPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            attachment.FilePath.TrimStart('/'));

        if (System.IO.File.Exists(physicalPath))
            System.IO.File.Delete(physicalPath);

        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
}
