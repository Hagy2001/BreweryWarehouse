namespace BreweryWarehouse.E2ETests;

/// <summary>
/// 10-step Playwright E2E scenario for the BeerStyle entity.
/// Prerequisites: the app must be running locally before executing this test.
///   dotnet run --project ../BreweryWarehouse.Web
/// Install browsers once with:
///   pwsh bin/Debug/net8.0/playwright.ps1 install chromium
/// </summary>
public class BeerStyleE2ETests : IAsyncLifetime
{
    // Base URL matches the http profile in launchSettings.json
    private const string BaseUrl = "http://localhost:5289";
    private const string AdminEmail = "admin@brewery.com";
    private const string AdminPassword = "Admin123!";

    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IBrowserContext _context = null!;
    private IPage _page = null!;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }

    [Fact]
    public async Task BeerStyle_FullLifecycle_10Steps()
    {
        const string testName = "Playwright Test Ale";
        const string testDescription = "A crisp golden ale created by the Playwright E2E suite.";
        const string updatedDescription = "Updated by Playwright — step 7 edit.";

        // ── Step 1: Navigate to the login page ────────────────────────────────
        await _page.GotoAsync($"{BaseUrl}/Identity/Account/Login");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.Contains("Sign In", await _page.TitleAsync());

        // ── Step 2: Log in with the seeded admin credentials ──────────────────
        await _page.GetByLabel("Email").FillAsync(AdminEmail);
        await _page.GetByLabel("Password").FillAsync(AdminPassword);
        await _page.GetByRole(AriaRole.Button, new() { Name = "Sign In", Exact = true }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // After login the app redirects to the dashboard (Home/Index)
        Assert.DoesNotContain("Sign In", await _page.TitleAsync());

        // ── Step 3: Navigate to the BeerStyle Index page ──────────────────────
        await _page.GotoAsync($"{BaseUrl}/beer-styles");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.Contains("Beer Style", await _page.TitleAsync());

        // ── Step 4: Click "New Beer Style", fill the form, and submit ─────────
        await _page.GetByRole(AriaRole.Link, new() { Name = "New Beer Style" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await _page.GetByLabel("Name", new() { Exact = true }).FillAsync(testName);
        // Exact = true disambiguates from the AI panel's "Your description" label on this page
        await _page.GetByLabel("Description", new() { Exact = true }).FillAsync(testDescription);
        await _page.GetByLabel("Alcohol %", new() { Exact = true }).FillAsync("5.2");
        await _page.GetByLabel("IBU", new() { Exact = true }).FillAsync("32");
        await _page.GetByLabel("Color (EBC)", new() { Exact = true }).FillAsync("8");
        await _page.GetByLabel("Category").SelectOptionAsync(new SelectOptionValue { Label = "Ale" });

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // ── Step 5: Assert the new BeerStyle appears in the Index list ─────────
        await _page.GotoAsync($"{BaseUrl}/beer-styles");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var indexContent = await _page.ContentAsync();
        Assert.Contains(testName, indexContent);

        // ── Step 6: Navigate to the Details page for the new record ───────────
        // Find the row that contains our test name, then follow its Details link
        var row = _page.Locator("tr").Filter(new LocatorFilterOptions { HasText = testName });
        var detailsAnchor = row.GetByRole(AriaRole.Link, new() { Name = "Details" });
        await detailsAnchor.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.Contains(testName, await _page.TitleAsync());

        // ── Step 7: Click Edit, change the Description, and save ──────────────
        await _page.GetByRole(AriaRole.Link, new() { Name = "Edit Style" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var descTextarea = _page.GetByLabel("Description");
        await descTextarea.ClearAsync();
        await descTextarea.FillAsync(updatedDescription);

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // ── Step 8: Assert the updated Description is reflected on Details ─────
        // Edit redirects back to the Index; navigate to Details via the list
        var updatedRow = _page.Locator("tr").Filter(new LocatorFilterOptions { HasText = testName });
        await updatedRow.GetByRole(AriaRole.Link, new() { Name = "Details" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var detailsContent = await _page.ContentAsync();
        Assert.Contains(updatedDescription, detailsContent);

        // ── Step 9: Delete the record through the confirmation flow ────────────
        // The Delete button is inside a form with onsubmit confirm(); we handle
        // the dialog by accepting it via Playwright's dialog event.
        _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();

        await _page.GetByRole(AriaRole.Button, new() { Name = "Delete Style" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // ── Step 10: Assert it no longer appears in the Index, then log out ────
        var finalIndexContent = await _page.ContentAsync();
        Assert.DoesNotContain(testName, finalIndexContent);

        // Log out via the Logout button in the sidebar
        await _page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // After logout the app redirects to the login page (returnUrl=/)
        Assert.Contains("Sign In", await _page.TitleAsync());
    }
}
