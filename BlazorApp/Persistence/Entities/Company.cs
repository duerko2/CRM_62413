namespace BlazorApp.Persistence.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Navigation property to User
    public virtual ICollection<User> Users { get; set; }

}