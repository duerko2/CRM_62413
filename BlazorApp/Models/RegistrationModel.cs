namespace BlazorApp.Models;

public class RegistrationModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Name { get; set; }
    public string Salt { get; set; }
}