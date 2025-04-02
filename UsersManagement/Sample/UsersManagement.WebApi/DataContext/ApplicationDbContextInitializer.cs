namespace UsersManagement.WebApi.DataContext;

public static class ApplicationDbContextInitializer
{
    public static async Task<WebApplication> InitialDatabaseAsync(this WebApplication  app)
    {
        
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeedInitializer>();

        // Initialise and seed the database
        await initialiser.ExecuteAsync();

        return app;
    }
}
