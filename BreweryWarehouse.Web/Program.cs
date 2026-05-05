using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Repositories;
using BreweryWarehouse.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BeerStyleRepository>();
builder.Services.AddScoped<CanRepository>();
builder.Services.AddScoped<KegRepository>();
builder.Services.AddScoped<WarehouseLocationRepository>();
builder.Services.AddScoped<StockEntryRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddDbContext<BreweryWarehouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BreweryWarehouseDbContext")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
