using BlazorApp.Components;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddHttpContextAccessor();


        builder.Services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

        builder.Services.AddAuthentication("CookieAuth")
            .AddCookie("CookieAuth", config =>
            {
                config.Cookie.Name = "UserLoginCookie";
                config.LoginPath = "/login";
            });

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }



        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            SeedDatabase(dbContext);
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }

    static void SeedDatabase(UserDbContext dbContext)
        {
            // Ensure the database is created
            dbContext.Database.EnsureCreated();

            // Check if there are any users, if not, create a default one
            if (!dbContext.Users.Any())
            {
                var defaultUser = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                };

                dbContext.Users.Add(defaultUser);
                dbContext.SaveChanges();
            }
        }
}