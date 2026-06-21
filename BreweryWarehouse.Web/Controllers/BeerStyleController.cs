using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BreweryWarehouse.Model;
using BreweryWarehouse.Web.Models;
using BreweryWarehouse.Web.Repositories;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace BreweryWarehouse.Web;

[Authorize]
[Route("beer-styles")]
public class BeerStyleController : Controller
{
    private readonly BeerStyleRepository repository;
    private readonly ILogger<BeerStyleController> _logger;
    private readonly OpenAIClient _openAiClient;

    public BeerStyleController(BeerStyleRepository repository, ILogger<BeerStyleController> logger, OpenAIClient openAiClient)
    {
        this.repository = repository;
        _logger = logger;
        _openAiClient = openAiClient;
    }

    [Route("")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(repository.GetAll());
    }

    [HttpGet]
    [Route("search")]
    [AllowAnonymous]
    public JsonResult Search(string? q)
    {
        IEnumerable<BeerStyle> beerStyles = repository.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            beerStyles = beerStyles.Where(style =>
                style.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        var results = beerStyles
            .OrderBy(style => style.Name)
            .Take(20)
            .Select(style => new
            {
                style.Id,
                style.Name,
                Category = style.Category.GetDescription(),
                style.AlcoholPercentage,
                style.IBU,
                style.ColorEBC
            })
            .ToList();

        return Json(results);
    }

    [Route("{id:int}/detail")]
    [Authorize]
    public IActionResult Details(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        return View(beerStyle);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("ai-quick-add")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public async Task<IActionResult> AiQuickAdd([FromForm] string prompt)
    {
        _logger.LogInformation("AI Quick Add requested by {User} with prompt: {Prompt}", User.Identity?.Name, prompt);

        if (string.IsNullOrWhiteSpace(prompt))
        {
            TempData["AiError"] = "Please enter a description before generating.";
            return View("Create", new BeerStyleCreateModel());
        }

        const string systemPrompt = """
            You are a beer data extraction assistant. Extract beer style information from the user's description and return ONLY a raw JSON object (no markdown fences, no commentary) with these exact fields:
            {
              "Name": string or null,
              "Description": string or null,
              "AlcoholPercentage": number or null,
              "IBU": integer or null,
              "ColorEBC": number or null,
              "Category": string or null
            }
            Valid Category values (pick the best match or null if unclear): Lager, Ale, IPA, Stout, Wheat, Sour, Porter.
            IMPORTANT: If the user does not mention a field, set it to null. Do NOT invent or guess values for unmentioned fields.
            """;

        try
        {
            var chatClient = _openAiClient.GetChatClient("deepseek-v4-flash");

            var completion = await chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(prompt)
                ],
                new ChatCompletionOptions
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat(),
                    MaxOutputTokenCount = 300,
                    Temperature = 0f
                });

            var json = completion.Value.Content[0].Text;

            BeerStyleCreateModel model;
            try
            {
                var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                model = new BeerStyleCreateModel
                {
                    Name = root.TryGetProperty("Name", out var name) && name.ValueKind != JsonValueKind.Null
                        ? name.GetString() ?? string.Empty : string.Empty,
                    Description = root.TryGetProperty("Description", out var desc) && desc.ValueKind != JsonValueKind.Null
                        ? desc.GetString() ?? string.Empty : string.Empty,
                    AlcoholPercentage = root.TryGetProperty("AlcoholPercentage", out var abv) && abv.ValueKind == JsonValueKind.Number
                        ? abv.GetDouble() : 0,
                    IBU = root.TryGetProperty("IBU", out var ibu) && ibu.ValueKind == JsonValueKind.Number
                        ? ibu.GetInt32() : 0,
                    ColorEBC = root.TryGetProperty("ColorEBC", out var ebc) && ebc.ValueKind == JsonValueKind.Number
                        ? ebc.GetDouble() : 0,
                    Category = root.TryGetProperty("Category", out var cat) && cat.ValueKind != JsonValueKind.Null
                        && Enum.TryParse<BeerCategory>(cat.GetString(), out var parsedCat)
                        ? parsedCat : default
                };
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "AI Quick Add: failed to parse JSON response for prompt: {Prompt}", prompt);
                TempData["AiError"] = "Couldn't process that response. Try rephrasing or fill the form in manually.";
                return View("Create", new BeerStyleCreateModel());
            }

            TryValidateModel(model);
            _logger.LogInformation("AI Quick Add: parsed model for {User} — valid={Valid}, Name={Name}", User.Identity?.Name, ModelState.IsValid, model.Name);
            return View("Create", model);
        }
        catch (ClientResultException ex) when (ex.Status == 401)
        {
            _logger.LogWarning(ex, "AI Quick Add: DeepSeek API returned 401 — check API key / account balance");
            TempData["AiError"] = "AI service unavailable (authentication error). Fill the form in manually.";
            return View("Create", new BeerStyleCreateModel());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "AI Quick Add: API call failed for prompt: {Prompt}", prompt);
            TempData["AiError"] = "Couldn't process that, try rephrasing or fill the form in manually.";
            return View("Create", new BeerStyleCreateModel());
        }
    }

    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create()
    {
        return View(new BeerStyleCreateModel());
    }

    [HttpPost]
    [ActionName("Create")]
    [ValidateAntiForgeryToken]
    [Route("create")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Create(BeerStyleCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("BeerStyle creation failed validation for {Name} by {User}", model.Name, User.Identity?.Name);
            return View(model);
        }

        BeerStyle beerStyle = new BeerStyle
        {
            Name = model.Name,
            Description = model.Description,
            AlcoholPercentage = model.AlcoholPercentage,
            IBU = model.IBU,
            ColorEBC = model.ColorEBC,
            Category = model.Category
        };

        repository.Add(beerStyle);
        _logger.LogInformation("BeerStyle {Id} '{Name}' created by {User}", beerStyle.Id, beerStyle.Name, User.Identity?.Name);

        return RedirectToAction("Index");
    }

    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        BeerStyleEditModel model = new BeerStyleEditModel
        {
            Id = beerStyle.Id,
            Name = beerStyle.Name,
            Description = beerStyle.Description,
            AlcoholPercentage = beerStyle.AlcoholPercentage,
            IBU = beerStyle.IBU,
            ColorEBC = beerStyle.ColorEBC,
            Category = beerStyle.Category
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/edit")]
    [Authorize(Roles = "Admin,WarehouseManager")]
    public IActionResult Edit(int id, BeerStyleEditModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("BeerStyle {Id} edit failed validation by {User}", id, User.Identity?.Name);
            return View(model);
        }

        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        beerStyle.Name = model.Name;
        beerStyle.Description = model.Description;
        beerStyle.AlcoholPercentage = model.AlcoholPercentage;
        beerStyle.IBU = model.IBU;
        beerStyle.ColorEBC = model.ColorEBC;
        beerStyle.Category = model.Category;

        repository.Update();
        _logger.LogInformation("BeerStyle {Id} '{Name}' updated by {User}", id, beerStyle.Name, User.Identity?.Name);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        BeerStyle? beerStyle = repository.GetById(id);

        if (beerStyle is null)
        {
            return NotFound();
        }

        repository.SoftDelete(beerStyle);
        _logger.LogInformation("BeerStyle {Id} '{Name}' soft-deleted by {User}", id, beerStyle.Name, User.Identity?.Name);

        return RedirectToAction("Index");
    }
}
