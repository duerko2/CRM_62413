using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using BlazorApp.Persistence;

namespace BlazorApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly CrmDbContext _db;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(IJSRuntime jsRuntime, CrmDbContext dbContext)
    {
        _db = dbContext;
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userId = await GetUserIdAsync();
        var username = await GetUsernameAsync();

        if (!userId.HasValue)
        {
            return new AuthenticationState(_anonymous);
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "Fake authentication");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    private async Task<int?> GetUserIdAsync()
    {
        try
        {
            var id = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "uid");
            var idInt = Int32.Parse(id);
            return idInt;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error fetching user ID: " + e.Message);
            return null;
        }
    }

    private async Task<string?> GetUsernameAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "username");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error fetching username: " + e.Message);
            return null;
        }
    }

    public async Task Login(string username)
    {
        try
        {
            // VERY Simple authentication
            var dbUser = _db.Users.SingleOrDefault(u => u.Name == username);
                
            if (dbUser == null)
            {
                throw new Exception("User not found");
            }
                
            // Authenticated! Stores the username and user ID in browser
                
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "username", username);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "uid", dbUser.Id);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Fake authentication");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during login: " + e.Message);
        }
    }

    public async Task Logout()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "username");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "uid");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during logout: " + e.Message);
        }
    }
}