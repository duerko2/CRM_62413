using BlazorApp.Models;
using Contact = BlazorApp.Persistence.Entities.Contact;

namespace BlazorApp.Repository;

public class InMemoryContactRepository : IContactRepository
{
    private List<Contact> _contacts;
    public InMemoryContactRepository()
    {
        InitializeContacts();
    }

    private void InitializeContacts()
    {
        _contacts = new List<Contact>();
        _contacts.Add(new Contact
        {
            Id = 1,
            Name = "John Doe",
            Address = "123 Elm St",
            UserId = 1,
        });
    }

    public Contact GetContact(int id)
    {
        return _contacts.Single(c => c.Id == id);
    }

    List<ContactListRow> IContactRepository.GetContactsForUser(int userId)
    {
        throw new NotImplementedException();
    }

    public void AddContact(Models.Contact contact)
    {
        throw new NotImplementedException();
    }

    public void UpdateContact(Models.Contact contact)
    {
        throw new NotImplementedException();
    }

    Models.Contact IContactRepository.GetContact(int id)
    {
        throw new NotImplementedException();
    }

    public List<Contact> GetContactsForUser(int userId)
    {
        return _contacts.Where(c => c.UserId == userId).ToList();
    }

    public void AddContact(Contact contact)
    {
        if (contact.Id != default || contact.Id == 0)
        {
            throw new ArgumentException("New contact must not have an Id");
        }
        contact.Id = _contacts.Max(c => c.Id) + 1;
        _contacts.Add(contact);
    }

    public void UpdateContact(Contact contact)
    {
        var existingContact = _contacts.Single(c => c.Id == contact.Id);
        existingContact.Name = contact.Name;
        existingContact.Address = contact.Address;
        existingContact.UserId = contact.UserId;
    }

    public void DeleteContact(int id)
    {
        _contacts.RemoveAll(c => c.Id == id);
    }

    public int GetCompanyIdForUser(int userId)
    {
        throw new NotImplementedException();
    }

    public List<ContactListRow> GetContactsForCompany(int companyId)
    {
        throw new NotImplementedException();
    }
}