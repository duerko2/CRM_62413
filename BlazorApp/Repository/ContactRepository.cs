using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Contact = BlazorApp.Persistence.Entities.Contact;

namespace BlazorApp.Repository;

public class ContactRepository : IContactRepository
{
    private readonly CrmDbContext _db;

    public ContactRepository(CrmDbContext db)
    {
        _db = db;
    }
        
    public Models.Contact GetContact(int id)
    {
        var contact = _db.Contacts.SingleOrDefault(c => c.Id == id);
        if (contact == default)
        {
            throw new InvalidOperationException("Contact not found");
        } 
        var model = ContactMapper.MapToModel(contact);
        _db.ChangeTracker.Clear();
        return model;
    }

    public List<ContactListRow> GetContactsForUser(int userId)
    {
        return _db.Contacts.Where(c => c.UserId == userId).Select(c => 
            new ContactListRow { 
                Id = c.Id, 
                Name = c.Name, 
                Type = (ContactType)c.Type, 
                Address = c.Address, 
                Company = c.Company, 
                VAT = c.VAT
            }).ToList();
    }

    public void AddContact(Models.Contact contact)
    {
        var entity = ContactMapper.MapToEntity(contact, contact.UserId);
        _db.Contacts.Add(entity);
        _db.SaveChanges();
        contact.Id = entity.Id;
    }

    public void UpdateContact(Models.Contact contact)
    {
        var entity = ContactMapper.MapToEntity(contact, contact.UserId);
        _db.Contacts.Update(entity);
        _db.SaveChanges();
    }

    public void DeleteContact(int id)
    {
        var contact = _db.Contacts.SingleOrDefault(c => c.Id == id);
        if (contact == default)
        {
            throw new InvalidOperationException("Contact not found");
        }
        _db.Contacts.Remove(contact);
        _db.SaveChanges();
    }
}