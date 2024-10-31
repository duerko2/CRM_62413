namespace BlazorApp.Persistence.Entities;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Company { get; set; }
    public string VAT { get; set; }
    public Byte Type { get; set; }  // Either 0 for "Lead" or 1 for "Customer"
    
    // Foreign key to User
    public int UserId { get; set; }
    
    // Navigation property to User
    public virtual User User { get; set; }
    
    public virtual ICollection<Person> Persons { get; set; }
    public virtual ICollection<Pipeline> Pipelines { get; set; }

}