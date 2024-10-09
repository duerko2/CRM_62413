namespace BlazorApp.Persistence.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Foreign key to Company
    public int CompanyId { get; set; }
    
    // Navigation property to Company
    public virtual Company Company { get; set; }
    
    // Navigation property back to Contact
    public virtual ICollection<Contact> Contacts { get; set; }
}