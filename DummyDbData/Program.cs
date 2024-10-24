using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DummyDbData;

class Program
{
    static void Main(string[] args)
    {
        // Setup DI and initialize DbContext
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<CrmDbContext>(options =>
            options.UseSqlServer("Default Connection")); // Replace with your connection string

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        using (var context = serviceProvider.GetRequiredService<CrmDbContext>())
        {
            // Check if the database is already populated
            if (!context.Contacts.Any())
            {
                SeedContactTable.SeedTable(context);
            }
        }

        Console.WriteLine("Database seeded successfully!");
    }
}