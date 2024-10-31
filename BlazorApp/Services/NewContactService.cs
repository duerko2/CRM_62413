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
        private readonly CrmDbContext _db;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public NewContactService(IContactRepository contactRepository, CrmDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _contactRepository = contactRepository;
            _db = db;
            _authenticationStateProvider = authenticationStateProvider;
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
            return ContactMapper.MapToModel(contact);
        }
        
        public Contact GetContact()
        {
            var user = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
            Console.WriteLine(user.FindFirst(claim => claim.Type == ClaimTypes.Name).Value);

            var contact = _db.Contacts.Where(c => c.Name == "Marcus").SingleOrDefault();

            if (contact == null)
            {
                return new Contact
                {
                    Name = "Marcus",
                    Persons = new List<Person>
                    {
                        new Person
                        {
                            Email = "123",
                            Name = "Marcus",
                            Phone = "+45 12345678"
                        }
                    },
                    Company = "Elgiganten A/S",
                    Address = "Herlev Hovedgade 45, 2730 Herlev, Denmark",
                    Notes = "VIP client, prefers email communication."
                };
            }
            return new Contact
            {
                Name = "Elgiganten",
                Persons = contact.Persons.Select(p => new Models.Person { Email = p.Email, Name = p.Name, Phone = p.PhoneNumber }).ToList(),
                Address = "Herlev Hovedgade 45, 2730 Herlev, Denmark",
                Company = "Elgiganten A/S",
                Notes = "VIP client, prefers email communication."
            };
        }

        /// <summary>
        /// Idempotent method to save a contact
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