using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Security.Claims;
using System.Threading.Tasks;

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
                // Check if we're running in WebAssembly mode before making JS interop calls
                var username = await GetUsernameAsync();
                if (string.IsNullOrEmpty(username))
                {
                    return new AuthenticationState(_anonymous);
                }

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }, "Fake authentication");

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            } 
            catch (InvalidOperationException e) 
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
            
        }

        public async Task Login(string username)
        {
            // Ensure the JS interop call happens after the component has rendered
            await _localStorage.SetItemAsync("username", username);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }, "Fake authentication");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("username");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        // Helper method to get username asynchronously to handle the interop properly
        private async Task<string> GetUsernameAsync()
        {
            // This ensures the JS interop call is handled safely
            return await _localStorage.GetItemAsStringAsync("username");
        }
    }
}
