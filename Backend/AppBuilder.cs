using Microsoft.EntityFrameworkCore;
using StreamerApi.Entities;

public static class AppBuilder
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var services = app.ApplicationServices.CreateScope();
        var dbContext = services.ServiceProvider.GetService<StreamerDbContext>();

        if (dbContext is null) 
        {
            throw new Exception("Db context not found.");
        }

        if(dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}