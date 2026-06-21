using Microsoft.AspNetCore.Authorization;

namespace BreweryWarehouse.Web.McpTools;

public class McpApiKeyRequirement : IAuthorizationRequirement { }

public class McpApiKeyHandler : AuthorizationHandler<McpApiKeyRequirement>
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _http;

    public McpApiKeyHandler(IConfiguration config, IHttpContextAccessor http)
    {
        _config = config;
        _http = http;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        McpApiKeyRequirement requirement)
    {
        var httpContext = _http.HttpContext;
        if (httpContext is null) { context.Fail(); return Task.CompletedTask; }

        var expected = _config["Mcp:ApiKey"];
        if (string.IsNullOrWhiteSpace(expected)) { context.Fail(); return Task.CompletedTask; }

        var provided = httpContext.Request.Headers["X-Mcp-Api-Key"].FirstOrDefault();
        if (provided == expected)
            context.Succeed(requirement);
        else
            context.Fail();

        return Task.CompletedTask;
    }
}
