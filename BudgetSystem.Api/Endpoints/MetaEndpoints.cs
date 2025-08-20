using BudgetSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetSystem.Api.Endpoints;

public static class MetaEndpoints
{
    public static IEndpointRouteBuilder MapMetaEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/meta").WithTags("Meta");

        g.MapGet("/ping", () => Results.Ok(new { status = "ok" }));

        g.MapGet("/dbcheck", async (AppDbContext db) =>
        {
            var canConnect = await db.Database.CanConnectAsync();
            return Results.Ok(new { database = canConnect ? "online" : "offline" });
        });

        g.MapGet("/dbinfo", (AppDbContext db) =>
        {
            var c = db.Database.GetDbConnection();
            return Results.Ok(new { server = c.DataSource, database = c.Database, provider = db.Database.ProviderName });
        });

        return app;
    }
}
