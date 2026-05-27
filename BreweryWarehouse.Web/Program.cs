using BreweryWarehouse.Web.Binders;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Repositories;
using BreweryWarehouse.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BreweryWarehouse.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider()));
builder.Services.AddScoped<BeerStyleRepository>();
builder.Services.AddScoped<CanRepository>();
builder.Services.AddScoped<KegRepository>();
builder.Services.AddScoped<WarehouseLocationRepository>();
builder.Services.AddScoped<StockEntryRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services
    .AddDefaultIdentity<AppUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BreweryWarehouseDbContext>();
builder.Services.AddDbContext<BreweryWarehouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BreweryWarehouseDbContext")));
builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BreweryWarehouseDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportedCultures = new[]
{
    new CultureInfo("hr"),
    new CultureInfo("en-US")
};
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("hr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() }
};
app.UseRequestLocalization(localizationOptions);
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}


app.Run();
