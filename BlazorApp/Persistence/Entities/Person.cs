namespace BlazorApp.Persistence.Entities;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    // Foreign key property
    public int ContactId { get; set; }

    // Navigation property back to Contact
    public virtual Contact Contact { get; set; }
}