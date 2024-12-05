using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository;

public class EFRegistrationRepository : IRegistrationRepository
{
    private readonly IDbContextFactory<CrmDbContext> _contextFactory;

    public EFRegistrationRepository(IDbContextFactory<CrmDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public bool RegistrationEmailExists(string registrationModelEmail)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.UserInvitations.Any(u => u.Email == registrationModelEmail);
    }

    public bool UserNameExists(string registrationModelUsername)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Users.Any(u => u.UserName == registrationModelUsername);
    }

    public void AddUser(RegistrationModel registrationModel)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var invitation = db.UserInvitations.FirstOrDefault(u => u.Email == registrationModel.Email);
        var user = new User
        {
            UserName = registrationModel.Username,
            Name = registrationModel.Name,
            Salt = registrationModel.Salt,
            Password = registrationModel.Password,
            Email = registrationModel.Email,
            Role = invitation.Role,
            CompanyId = invitation.CompanyId
        };
        db.Users.Add(user);
        db.SaveChanges();
    }
}