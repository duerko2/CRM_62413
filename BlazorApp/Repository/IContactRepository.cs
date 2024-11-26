using BlazorApp.Models;

namespace BlazorApp.Repository;

public interface IContactRepository
{
    public Contact GetContact(int id);
    public List<ContactListRow> GetContactsForUser(int userId);
    
    public void AddContact(Contact contact);
    public void UpdateContact(Contact contact);
    public void DeleteContact(int id);
    int GetCompanyIdForUser(int userId);
    List<ContactListRow> GetContactsForCompany(int companyId);
}