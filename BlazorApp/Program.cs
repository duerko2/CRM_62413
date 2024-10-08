using BlazorApp.Components;
using BlazorApp.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Components.Authorization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<ContactService>();
        builder.Services.AddScoped<SideMenuService>();
        builder.Services.AddScoped<BlazorApp.Services.CampaignService>();

        
        
        builder.Services.AddDbContext<CrmDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default Connection")));
        



        var app = builder.Build();
        
        Db.Configure(app.Services);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}