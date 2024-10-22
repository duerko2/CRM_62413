using System.Security.Claims;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.AspNetCore.Components.Authorization;


namespace BlazorApp.Services
{        
    //For demonstration purposes, we'll change it to data from a database later
    public class ContactService
    {
        private readonly CrmDbContext _db;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ContactService(CrmDbContext db, AuthenticationStateProvider authenticationStateProvider)
        {
            _db = db;
            _authenticationStateProvider = authenticationStateProvider;
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
                Persons = contact.Persons.Select(p=> new Models.Person { Email = p.Email, Name = p.Name, Phone = p.PhoneNumber}).ToList(),
                Address = "Herlev Hovedgade 45, 2730 Herlev, Denmark",
                Company = "Elgiganten A/S",
                Notes = "VIP client, prefers email communication."
            };
        }
        
        /// <summary>
        /// Idempotent method to save a contact
        /// </summary>
        /// <param name="contact"></param>
        public void SaveContact(Contact contact)
        {
            var user = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
            
            Console.WriteLine();
            
            Persistence.Entities.Contact newContact = new Persistence.Entities.Contact
            {
                Name = contact.Name,
                Address = contact.Address,
                UserId = 2,
            };
            _db.Contacts.Add(newContact);
            _db.SaveChanges();
            _db.Persons.AddRange(contact.Persons.Select(p => new Persistence.Entities.Person
            {
                Name = p.Name, 
                Email = p.Email, 
                PhoneNumber = p.Phone, 
                ContactId = newContact.Id
            }));
            _db.SaveChanges();
        }
    }
}
