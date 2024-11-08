using System.Security.Claims;
using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Repository;
using Microsoft.AspNetCore.Components.Authorization;


namespace BlazorApp.Services
{
    //For demonstration purposes, we'll change it to data from a database later
    public class NewContactService
    {
        private readonly IContactRepository _contactRepository;

        public NewContactService(IContactRepository contactRepository, CrmDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _contactRepository = contactRepository;
        }
        
        /// <summary>
        /// Retrieves the list of contacts.
        /// </summary>
        /// <returns>List of contacts.</returns>
        public List<Contact> GetContacts(int userId)
        {
            Console.WriteLine("Getting contacts for user: " + userId);
            var contacts = _contactRepository.GetContactsForUser(userId);
            return contacts.Select(ContactMapper.MapToModel).ToList();
        }
        
        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact with the specified ID, or null if not found.</returns>
        public Contact GetContactById(int id)
        {
            var contact = _contactRepository.GetContact(id);
            var contactModel = ContactMapper.MapToModel(contact);
            
            return contactModel;
        }

        /// <summary>
        /// Idempotent method to save a contact and its related Persons.
        /// </summary>
        /// <param name="contact"></param>
        public async void SaveContact(Contact contact, int userId)
        {
            var contactEntity = ContactMapper.MapToEntity(contact, userId);
            
            if(contactEntity.Id == default)
            {
                _contactRepository.AddContact(contactEntity);
            }
            else
            {
                _contactRepository.UpdateContact(contactEntity);
            }
        }
    }
}