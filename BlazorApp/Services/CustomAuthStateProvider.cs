using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BlazorApp.Persistence;

namespace BlazorApp.Services
{    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userId = GetUserIdAsync();
                var username = GetUsernameAsync();
                Console.Write(userId.Result);
                Console.Write(username.Result);
                
                if (!userId.Result.HasValue)
                {
                    return new AuthenticationState(_anonymous);
                }

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username.Result),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.NameIdentifier, userId.Result.ToString())
                }, "Fake authentication");

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            } 
            catch (InvalidOperationException e) 
            {
                Console.WriteLine("Invalid operation exception");
                return new AuthenticationState(new ClaimsPrincipal());
            }
            
        }

        private async Task<int?> GetUserIdAsync()
        {
            return 2;
            var getUserIdTask = _localStorage.GetItemAsync<int>("uid").AsTask();

            // Create a timeout task that completes after 2 seconds
            var timeoutTask = Task.Delay(2000);

            // Wait for either the user ID task to complete or the timeout to expire
            var completedTask = await Task.WhenAny(getUserIdTask, timeoutTask);

            // If the completed task is the user ID task, return its result; otherwise, return null for a timeout
            if (completedTask == getUserIdTask)
            {
                return await getUserIdTask; // Successful completion
            }
            else
            {
                Console.WriteLine("Timeout occurred");
                return 2;
            }
        }

        public async Task Login(string username)
        {
            // Ensure the JS interop call happens after the component has rendered
            await _localStorage.SetItemAsync("username", username);
            await _localStorage.SetItemAsync<int>("uid", 2);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Fake authentication");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("username");
            await _localStorage.RemoveItemAsync("uid");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        // Helper method to get username asynchronously to handle the interop properly
        private async Task<string> GetUsernameAsync()
        {
            // This ensures the JS interop call is handled safely
            return "username";
            return await _localStorage.GetItemAsStringAsync("username");
        }
    }
}
