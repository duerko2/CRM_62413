namespace BlazorApp.Persistence.Entities;

public class UserInvitation
{
    public int Id { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public string Role { get; set; }
    public virtual Company Company { get; set; }
}