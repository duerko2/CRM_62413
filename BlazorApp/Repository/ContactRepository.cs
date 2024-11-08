using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public class ContactRepository : IContactRepository
{
    private readonly CrmDbContext _db;

    public ContactRepository(CrmDbContext db)
    {
        _db = db;
    }
        
    public Contact GetContact(int id)
    {
        var contact = _db.Contacts.SingleOrDefault(c => c.Id == id);
        if (contact == default)
        {
            throw new InvalidOperationException("Contact not found");
        } 
        _db.ChangeTracker.Clear();
        return contact;
    }

    public List<Contact> GetContactsForUser(int userId)
    {
        return _db.Contacts.Where(c => c.UserId == userId).ToList();
    }

    public void AddContact(Contact contact)
    {
        _db.Contacts.Add(contact);
        _db.SaveChanges();
        _db.ChangeTracker.Clear();
    }

    public void UpdateContact(Contact contact)
    {
        _db.Contacts.Update(contact);
        _db.SaveChanges();
        _db.ChangeTracker.Clear();
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
        _db.ChangeTracker.Clear();
    }
}