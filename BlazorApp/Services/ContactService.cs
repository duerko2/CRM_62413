using System;
using System.Collections.Generic;
using System.Linq;
using BlazorApp.Models;

namespace BlazorApp.Services
{
    /// <summary>
    /// Service for managing contacts.
    /// </summary>
    public class ContactService
    {
        // List to store contacts
        private List<Contact> contacts;

        /// <summary>
        /// Constructor initializes the contacts list.
        /// </summary>
        public ContactService()
        {
            // Initialize contacts with sample data
            contacts = InitializeContacts();
        }

        /// <summary>
        /// Retrieves the list of contacts.
        /// </summary>
        /// <returns>List of contacts.</returns>
        public List<Contact> GetContacts()
        {
            return contacts;
        }

        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact with the specified ID, or null if not found.</returns>
        public Contact GetContactById(int id)
        {
            // Fetch contact by ID
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Updates a contact's information.
        /// </summary>
        /// <param name="updatedContact">The contact with updated information.</param>
        public void UpdateContact(Contact updatedContact)
        {
            // Find the index of the existing contact
            var index = contacts.FindIndex(c => c.Id == updatedContact.Id);

            if (index != -1)
            {
                // Update the contact in the list
                contacts[index] = updatedContact;
            }
            else
            {
                // Handle case where contact is not found
                // For example, add the new contact to the list
                contacts.Add(updatedContact);
            }
        }

        /// <summary>
        /// Initializes the contacts list with sample data.
        /// </summary>
        /// <returns>List of sample contacts.</returns>
        private List<Contact> InitializeContacts()
        {
            var sampleContacts = new List<Contact>
            {
                new Contact
                {
                    Id = 1,
                    Name = "TechCorp",
                    Persons = new List<Person>
                    {
                        new Person { Name = "John Doe", Email = "john.doe@techcorp.com", Phone = "+45 12345678" }
                    },
                    Address = "Tech Street 1, 1000 Copenhagen, Denmark",
                    Company = "TechCorp",
                    Notes = "Looking for cloud solutions.",
                    Type = "Lead"
                },
                new Contact
                {
                    Id = 2,
                    Name = "Smart Solutions",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Jane Smith", Email = "jane.smith@smartsolutions.com", Phone = "+45 87654321" }
                    },
                    Address = "Smart Avenue 7, 2000 Frederiksberg, Denmark",
                    Company = "Smart Solutions",
                    Notes = "Potential buyer for smart technologies.",
                    Type = "Lead"
                },
                new Contact
                {
                    Id = 3,
                    Name = "Innovatech",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Tom Johnson", Email = "tom.johnson@innovatech.com", Phone = "+45 22334455" }
                    },
                    Address = "Innovate Lane 2, 1500 Copenhagen, Denmark",
                    Company = "Innovatech",
                    Notes = "Interested in AI solutions.",
                    Type = "Lead"
                },
                new Contact
                {
                    Id = 4,
                    Name = "Visionary Labs",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Anna Brown", Email = "anna.brown@visionlabs.com", Phone = "+45 55667788" }
                    },
                    Address = "Vision Street 9, 1700 Copenhagen, Denmark",
                    Company = "Visionary Labs",
                    Notes = "Interested in cloud and big data services.",
                    Type = "Lead"
                },
                new Contact
                {
                    Id = 5,
                    Name = "NextGen IT",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Mark Davis", Email = "mark.davis@nextgenit.com", Phone = "+45 11223344" }
                    },
                    Address = "NextGen Boulevard 11, 3000 Helsingør, Denmark",
                    Company = "NextGen IT",
                    Notes = "Currently evaluating IT solutions.",
                    Type = "Lead"
                },
                new Contact
                {
                    Id = 6,
                    Name = "CloudTech",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Alice Green", Email = "alice.green@cloudtech.com", Phone = "+45 44556677" }
                    },
                    Address = "Cloud Street 15, 2300 Copenhagen, Denmark",
                    Company = "CloudTech",
                    Notes = "Existing customer for cloud storage.",
                    Type = "Customer"
                },
                new Contact
                {
                    Id = 7,
                    Name = "DataCorp",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Peter White", Email = "peter.white@datacorp.com", Phone = "+45 99887766" }
                    },
                    Address = "Data Avenue 8, 2500 Valby, Denmark",
                    Company = "DataCorp",
                    Notes = "Existing customer for data analytics services.",
                    Type = "Customer"
                },
                new Contact
                {
                    Id = 8,
                    Name = "SysAdminPro",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Sara Blue", Email = "sara.blue@sysadminpro.com", Phone = "+45 55669988" }
                    },
                    Address = "SysAdmin Street 21, 2200 Nørrebro, Denmark",
                    Company = "SysAdminPro",
                    Notes = "Long-term client for system administration tools.",
                    Type = "Customer"
                },
                new Contact
                {
                    Id = 9,
                    Name = "SecureNet",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Michael Black", Email = "michael.black@securenet.com", Phone = "+45 33445566" }
                    },
                    Address = "Security Lane 5, 2400 Copenhagen, Denmark",
                    Company = "SecureNet",
                    Notes = "Existing customer for network security solutions.",
                    Type = "Customer"
                },
                new Contact
                {
                    Id = 10,
                    Name = "GreenEnergy Inc.",
                    Persons = new List<Person>
                    {
                        new Person { Name = "Chris Red", Email = "chris.red@greenenergy.com", Phone = "+45 22335544" }
                    },
                    Address = "Energy Street 33, 4000 Roskilde, Denmark",
                    Company = "GreenEnergy Inc.",
                    Notes = "Renewable energy solutions customer.",
                    Type = "Customer"
                }
            };

            // Assign campaigns, comments, and activities to each contact
            foreach (var contact in sampleContacts)
            {
                contact.Campaigns = GetSampleCampaigns(contact.Id);
                contact.Comments = GetSampleComments(contact.Id);
                contact.Activities = GetSampleActivities(contact.Id);
            }

            return sampleContacts;
        }

        /// <summary>
        /// Generates sample campaigns for a contact.
        /// </summary>
        /// <param name="contactId">The ID of the contact.</param>
        /// <returns>List of sample campaigns.</returns>
        private List<Campaign> GetSampleCampaigns(int contactId)
        {
            // For demonstration, assign some campaigns based on the contactId
            if (contactId % 2 == 0)
            {
                // Even IDs get these campaigns
                return new List<Campaign>
                {
                    new Campaign { Id = 1, Name = "Summer Promotion" },
                    new Campaign { Id = 2, Name = "Winter Sale" }
                };
            }
            else
            {
                // Odd IDs get these campaigns
                return new List<Campaign>
                {
                    new Campaign { Id = 3, Name = "Spring Launch" },
                    new Campaign { Id = 4, Name = "Autumn Deals" }
                };
            }
        }

        /// <summary>
        /// Generates sample comments for a contact.
        /// </summary>
        /// <param name="contactId">The ID of the contact.</param>
        /// <returns>List of sample comments.</returns>
        private List<Comment> GetSampleComments(int contactId)
        {
            // Generate some sample comments
            return new List<Comment>
            {
                new Comment
                {
                    Date = DateTime.Now.AddDays(-contactId),
                    Text = "Initial contact made."
                },
                new Comment
                {
                    Date = DateTime.Now.AddDays(-contactId + 1),
                    Text = "Sent proposal."
                }
            };
        }

        /// <summary>
        /// Generates sample activities for a contact.
        /// </summary>
        /// <param name="contactId">The ID of the contact.</param>
        /// <returns>List of sample activities.</returns>
        private List<Activity> GetSampleActivities(int contactId)
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                    Date = DateTime.Now.AddDays(-1),
                    Description = "Comment added: 'Doesn't take the phone.'"
                },
                new Activity
                {
                    Date = DateTime.Now.AddDays(-3),
                    Description = "Placed order"
                },
                new Activity
                {
                    Date = DateTime.Now.AddDays(-10),
                    Description = "Converted from lead to customer"
                },
                new Activity
                {
                    Date = DateTime.Now.AddDays(-15),
                    Description = "Initial contact made"
                }
            };

            return activities;
        }
    }
}







/*
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
*/