using BlazorApp.Components;
using BlazorApp.Persistence;
using BlazorApp.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddDbContext<CrmDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Default Connection")));


        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<NewContactService>();
        builder.Services.AddScoped<SideMenuService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddSingleton<CampaignService>();
        builder.Services.AddSingleton<PipelineService>();
        builder.Services.AddSingleton<ContactService>();



        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();

        
        




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

        await app.RunAsync();
    }
}