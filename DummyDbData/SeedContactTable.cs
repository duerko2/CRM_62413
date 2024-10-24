using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;

namespace DummyDbData;

public static class SeedContactTable
{
    public static void SeedTable(CrmDbContext db)
    {
        db.Contacts.AddRange(
            new Contact
            {
                Name = "John",
                Address = "123 Main St",
                UserId = 2
            }, new Contact
            {
                Name = "Jane",
                Address = "456 Elm St",
                UserId = 2
            }, new Contact
            {
                Name = "Jim",
                Address = "789 Oak St",
                UserId = 2
            });
    }
}