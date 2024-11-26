using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models;

public class NewUser
{
    [Required(ErrorMessage = "Username is required")]
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public string Role { get; set; }
}