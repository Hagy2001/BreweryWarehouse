using BreweryWarehouse.Web.Binders;
using BreweryWarehouse.Web.Data;
using BreweryWarehouse.Web.McpTools;
using BreweryWarehouse.Web.Repositories;
using BreweryWarehouse.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using System.Globalization;
using BreweryWarehouse.Web.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;
using OpenAI;

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
            if (context.Request.Path.StartsWithSegments("/api") ||
                context.Request.Path.StartsWithSegments("/mcp"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api") ||
                context.Request.Path.StartsWithSegments("/mcp"))
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
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("BreweryWarehouseDbContext"),
            sqlOptions => sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)));
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IAuthorizationHandler, McpApiKeyHandler>();
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("McpApiKey", policy =>
            policy.AddRequirements(new McpApiKeyRequirement()));
    });
    builder.Services
        .AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly();
    var deepSeekApiKey = builder.Configuration["DeepSeek:ApiKey"] ?? string.Empty;
    builder.Services.AddSingleton(new OpenAIClient(
        new System.ClientModel.ApiKeyCredential(deepSeekApiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://api.deepseek.com") }));

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
        try
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BreweryWarehouseDbContext>();
            await DatabaseSeeder.SeedAsync(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Database seeding failed — app will continue without seed data");
        }
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
    app.MapMcp("/mcp").RequireAuthorization("McpApiKey");

    try
    {
        using var scope = app.Services.CreateScope();
        await IdentitySeeder.SeedAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Identity seeding failed — app will continue without seeded roles/users");
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
