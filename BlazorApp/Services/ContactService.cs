using System.Security.Claims;
using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using BlazorApp.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using Contact = BlazorApp.Models.Contact;


namespace BlazorApp.Services
{
    public class ContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IActivityLogRepository _activityLogRepository;

        public ContactService(IContactRepository contactRepository, IActivityLogRepository activityLogRepository)
        {
            _contactRepository = contactRepository;
            _activityLogRepository = activityLogRepository;
        }
        
        public List<ContactListRow> GetContacts(int userId)
        {
            var contacts = _contactRepository.GetContactsForUser(userId);
            return contacts;
        }
        
        public Contact GetContactById(int id)
        {
            var contact = _contactRepository.GetContact(id);
            
            return contact;
        }

        public void SaveContact(Contact contact, int userId)
        {
            contact.UserId = contact.UserId == default ? userId : contact.UserId;
            
            if(contact.Id == default)
            {
                _contactRepository.AddContact(contact);
                var contactId = contact.Id;
                _activityLogRepository.AddActivityLog(new ActivityLog {ContactId = contactId, Date = DateTime.Now, Type = (int)ActivityLogType.ContactCreated});
            }
            else
            {
                var contactId = contact.Id;
                _contactRepository.UpdateContact(contact);
                _activityLogRepository.AddActivityLog(new ActivityLog {ContactId = contactId, Date = DateTime.Now, Type = (int)ActivityLogType.ContactUpdated});
            }
        }

        public List<ContactListRow> GetContactsForCompany(int userId)
        {
            Console.WriteLine("fetching all users for userId:" + userId);
            var companyId = _contactRepository.GetCompanyIdForUser(userId);
            Console.WriteLine("fetching all users for companyId:" + companyId);
            var contacts = _contactRepository.GetContactsForCompany(companyId);
            return contacts;
        }
    }
}