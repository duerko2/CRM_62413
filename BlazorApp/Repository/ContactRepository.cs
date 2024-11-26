using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository;

public class ContactRepository : IContactRepository
{
    private readonly IDbContextFactory<CrmDbContext> _contextFactory;

    public ContactRepository(IDbContextFactory<CrmDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
        
    public Contact GetContact(int id)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var contact = db.Contacts.SingleOrDefault(c => c.Id == id);
        if (contact == default)
        {
            throw new InvalidOperationException("Contact not found");
        } 
        var model = ContactMapper.MapToModel(contact);
        db.ChangeTracker.Clear();
        return model;
    }

    public List<ContactListRow> GetContactsForUser(int userId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Contacts.Where(c => c.UserId == userId).Select(c => 
            new ContactListRow { 
                Id = c.Id, 
                Name = c.Name, 
                Type = (ContactType)c.Type, 
                Address = c.Address, 
                Company = c.Company, 
                VAT = c.VAT
            }).ToList();
    }

    public void AddContact(Contact contact)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var entity = ContactMapper.MapToEntity(contact, contact.UserId);
        db.Contacts.Add(entity);
        db.SaveChanges();
        contact.Id = entity.Id;
    }

    public void UpdateContact(Contact contact)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var entity = ContactMapper.MapToEntity(contact, contact.UserId);
        db.Contacts.Update(entity);
        db.SaveChanges();
    }

    public void DeleteContact(int id)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var contact = db.Contacts.SingleOrDefault(c => c.Id == id);
        if (contact == default)
        {
            throw new InvalidOperationException("Contact not found");
        }
        db.Contacts.Remove(contact);
        db.SaveChanges();
    }

    public int GetCompanyIdForUser(int userId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var user = db.Users.SingleOrDefault(u => u.Id == userId);
        if (user == default)
        {
            throw new InvalidOperationException("User not found");
        }
        return user.CompanyId;
    }

    public List<ContactListRow> GetContactsForCompany(int companyId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Contacts.Where(c => c.User.CompanyId == companyId).Select(c => 
            new ContactListRow { 
                Id = c.Id, 
                Name = c.Name, 
                Type = (ContactType)c.Type, 
                Address = c.Address, 
                Company = c.Company, 
                RepName = c.User.Name,
                VAT = c.VAT
            }).ToList();
    }
}