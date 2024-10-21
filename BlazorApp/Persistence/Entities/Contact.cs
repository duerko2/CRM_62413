namespace BlazorApp.Persistence.Entities;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    // Foreign key to User
    public int UserId { get; set; }
    
    // Navigation property to Company
    public virtual User User { get; set; }
    public virtual ICollection<Person> Persons { get; set; }

}