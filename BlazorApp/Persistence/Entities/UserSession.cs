namespace BlazorApp.Persistence.Entities;

public class UserSession
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}