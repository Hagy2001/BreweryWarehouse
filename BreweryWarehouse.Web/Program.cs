using BreweryWarehouse.Web.Binders;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.Repositories;
using BreweryWarehouse.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BreweryWarehouse.Web.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // On Azure App Service (Linux), /home is persistent storage; locally use relative logs/
    var logPath = Directory.Exists("/home")
        ? "/home/LogFiles/Application/log-.txt"
        : "logs/log-.txt";

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14));

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

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

    builder.Services
        .AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        });
    builder.Services.AddDbContext<BreweryWarehouseDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("BreweryWarehouseDbContext")));
    builder.Services.AddAuthorization();
    builder.Services.AddRazorPages();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "BreweryWarehouse API",
            Version = "v1"
        });

        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;
            return methodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Mvc.ApiControllerAttribute>()
                .Any() ?? false;
        });
    });

    var app = builder.Build();

    if (!app.Environment.IsEnvironment("Testing"))
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BreweryWarehouseDbContext>();
        await DatabaseSeeder.SeedAsync(context);
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
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
        RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()]
    };
    app.UseRequestLocalization(localizationOptions);
    app.UseStaticFiles();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
