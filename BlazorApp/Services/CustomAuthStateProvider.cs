using System.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Security.Cryptography;
using BlazorApp.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly CrmDbContext _db;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private ClaimsPrincipal _user;

    public CustomAuthStateProvider(IJSRuntime jsRuntime, CrmDbContext dbContext)
    {
        _db = dbContext;
        _jsRuntime = jsRuntime;
        AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authState = task.Result;
        _user = authState.User;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_user != default)
            return new AuthenticationState(_user);
        
        Console.WriteLine("Authenticating....");
        var userId = await GetUserIdAsync();
        var username = await GetUsernameAsync();

        if (!userId.HasValue)
        {
            return new AuthenticationState(_anonymous);
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User"),
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

    public async Task Login(string username, string password)
    {
        try
        {
            // VERY Simple authentication
            var dbUser = _db.Users.SingleOrDefault(u => u.UserName == username);
            
            if (dbUser == null)
            {
                throw new Exception("User not found");
            }
            
            // Retrieve salt from user
            string userSalt = dbUser.Salt;
            
            // Salt the entered password
            password += userSalt;
            
            // Hash the salted password            
            var sha256 = SHA256.Create();
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashedPassword = sha256.ComputeHash(passwordBytes);
            var hashedPasswordString = Convert.ToBase64String(hashedPassword);
            
            // Compare the salted+hashed password with the stored value
            string storedUserHashedPassword = dbUser.Password; 
            if (hashedPasswordString != storedUserHashedPassword)
            {
                throw new Exception("Wrong password");
            }

            var role = dbUser.Role;
            var userId = dbUser.Id;

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
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