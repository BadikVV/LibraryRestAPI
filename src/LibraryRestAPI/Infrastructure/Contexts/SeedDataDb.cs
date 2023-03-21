namespace LibraryRestAPI.Infrastructure.Contexts;

public class SeedDataDb
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataDbContext>();
        context.Database.EnsureCreated();
    }
}