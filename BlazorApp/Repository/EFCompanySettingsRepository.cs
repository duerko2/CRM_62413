using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository;

public class EFCompanySettingsRepository : ICompanySettingsRepository
{
    private readonly IDbContextFactory<CrmDbContext> _contextFactory;
    public EFCompanySettingsRepository(IDbContextFactory<CrmDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public bool UserExists(string newUserUsername)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Users.Any(u => u.UserName == newUserUsername);
    }

    public void AddUser(NewUser newUser)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var user = new UserInvitation
        {
            Email = newUser.Email,
            CompanyId = newUser.CompanyId,
            Role = newUser.Role
        };
        db.UserInvitations.Add(user);
        db.SaveChanges();
    }

    public int GetCompanyIdForUserId(int userId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Users.First(u => u.Id == userId).CompanyId;
    }
}