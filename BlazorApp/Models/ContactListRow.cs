namespace BlazorApp.Models;

public class ContactListRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Company { get; set; }
    public string RepName { get; set; }
    public string VAT { get; set; }
    public ContactType Type { get; set; }
}