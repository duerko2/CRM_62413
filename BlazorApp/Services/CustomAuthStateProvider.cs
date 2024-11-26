using System.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Security.Cryptography;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly CrmDbContext _db;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private ClaimsPrincipal _user;
    private CancellationTokenSource cts;

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
        if (_user == default)
        {
            cts = new CancellationTokenSource(1500);
            var jsTask = _jsRuntime.InvokeAsync<string>("localStorage.getItem", cts.Token, "CRMToken");
            var token = await jsTask;
            if (token != null)
            {
                var dbUserSession = _db.UserSessions.SingleOrDefault(us => us.Token == token);
                if (dbUserSession != null)
                {
                    var dbUser = _db.Users.SingleOrDefault(u => u.Id == dbUserSession.UserId);
                    if (dbUser != null)
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, dbUser.UserName),
                            new Claim(ClaimTypes.Role, dbUser.Role),
                            new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString())
                        }, "Authentication");
                        _user = new ClaimsPrincipal(identity);
                        return new AuthenticationState(_user);
                    }
                }
            }
        }
        return new AuthenticationState(_anonymous);
    }

    public async Task Login(string username, string password)
    {
        try
        {
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
            }, "Authentication");

            var user = new ClaimsPrincipal(identity);
            
            Guid token = Guid.NewGuid();
            _db.UserSessions.Add(new UserSession
            {
                UserId = userId,
                Token = token.ToString(),
                ExpirationDate = DateTime.Now.AddDays(1)
            });
            cts = new CancellationTokenSource(1500);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", cts.Token, "CRMToken", token);
            _db.SaveChanges();
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
            cts = new CancellationTokenSource(1500);
            var jsTask = _jsRuntime.InvokeAsync<string>("localStorage.getItem", cts.Token, "CRMToken");
            var token = await jsTask;
            var dbUserSession = _db.UserSessions.SingleOrDefault(us => us.Token == token);
            _db.UserSessions.Remove(dbUserSession);
            _db.SaveChanges();
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", cts.Token, "CRMToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during logout: " + e.Message);
        }
    }
}