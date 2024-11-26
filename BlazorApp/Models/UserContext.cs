using System.Security.Claims;

namespace BlazorApp.Models;

public class UserContext
{
    public int UserId { get; set; }
    public ClaimsPrincipal User { get; set; }
}