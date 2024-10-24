using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public interface IContactRepository
{
    public Contact GetContact(int id);
    public List<Contact> GetContactsForUser(int userId);
    
    public void AddContact(Contact contact);
    public void UpdateContact(Contact contact);
    public void DeleteContact(int id);
}