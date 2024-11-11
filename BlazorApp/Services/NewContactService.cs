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
    //For demonstration purposes, we'll change it to data from a database later
    public class NewContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IActivityLogRepository _activityLogRepository;

        public NewContactService(IContactRepository contactRepository, IActivityLogRepository activityLogRepository, CrmDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _contactRepository = contactRepository;
            _activityLogRepository = activityLogRepository;
        }
        
        /// <summary>
        /// Retrieves the list of contacts.
        /// </summary>
        /// <returns>List of contacts.</returns>
        public List<ContactListRow> GetContacts(int userId)
        {
            var contacts = _contactRepository.GetContactsForUser(userId);
            return contacts;
        }
        
        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact with the specified ID, or null if not found.</returns>
        public Contact GetContactById(int id)
        {
            var contact = _contactRepository.GetContact(id);
            
            return contact;
        }

        /// <summary>
        /// Idempotent method to save a contact and its related Persons.
        /// </summary>
        /// <param name="contact"></param>
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
    }
}