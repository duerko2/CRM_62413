using BlazorApp.Components;
using BlazorApp.Persistence;
using BlazorApp.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore;
using RolesAndPermissions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddServerSideBlazor()
        .AddCircuitOptions(options => { options.DetailedErrors = true; });


        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddDbContextFactory<CrmDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Default Connection")));

        builder.Services.AddDbContext<CrmDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Default Connection")));


        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddAuthorizationCore();
        
        // Add services to the container.
        builder.Services.AddScoped<ContactService>();
        builder.Services.AddScoped<SideMenuService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddScoped<CampaignService>();
        builder.Services.AddScoped<PipelineService>();
        builder.Services.AddScoped<ContactService>();
        builder.Services.AddScoped<CompanySettingsService>();



        // Add repositories to the container. Use Scoped lifetime to make a new instance of the repository for each request.
        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
        builder.Services.AddScoped<IActivityLogRepository, EFActivityLogRepository>();
        builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
        builder.Services.AddScoped<IPipelineRepository, PipelineRepository>();
        builder.Services.AddScoped<ICompanySettingsRepository, EFCompanySettingsRepository>();






        // Add permissions class library
        builder.Services.AddSingleton<IUserAccessManager, UserAccessManager>();

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