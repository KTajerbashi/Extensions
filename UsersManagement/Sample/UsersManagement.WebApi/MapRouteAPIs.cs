using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsersManagement.WebApi;

public static class MapRouteAPIs
{
    public static WebApplication MapRoutesAPIsRouter(this WebApplication app)
    {
        //  Cookie Health Checks:
        app.MapGet("/auth/check", (HttpContext context) => context.User.Identity?.IsAuthenticated == true);

        //  Token Validation Endpoint:
        app.MapPost("/auth/validate", [Authorize] (HttpContext context) => new { UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) });

        //  Session Monitoring:
        app.MapGet("/session/info", (HttpContext context) => new { SessionId = context.Session.Id, Keys = context.Session.Keys });

        return app;
    }
}


//tajerbashi
//@Kamran#123