using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDbContextFactory<CrmDbContext> _contextFactory;

        public ContactRepository(IDbContextFactory<CrmDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Contact GetContact(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var contact = db.Contacts
                           .Include(c => c.Persons)
                           .Include(c => c.Pipelines)
                           .Include(c => c.Comments)
                           .Include(c => c.ActivityLogs)
                           .SingleOrDefault(c => c.Id == id);
            if (contact == default)
            {
                throw new InvalidOperationException("Contact not found");
            }
            var model = ContactMapper.MapToModel(contact);
            db.ChangeTracker.Clear();
            return model;
        }

        public List<ContactListRow> GetContactsForUser(int userId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Contacts
                     .Where(c => c.UserId == userId)
                     .Select(c =>
                         new ContactListRow
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Type = (ContactType)c.Type,
                             Address = c.Address,
                             Company = c.Company,
                             VAT = c.VAT
                         })
                     .ToList();
        }

        public void AddContact(Contact contact)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = ContactMapper.MapToEntity(contact, contact.UserId);
            db.Contacts.Add(entity);
            db.SaveChanges();
            contact.Id = entity.Id;
        }

        /*
        public void UpdateContact(Contact contact)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = ContactMapper.MapToEntity(contact, contact.UserId);
            db.Contacts.Update(entity);
            db.SaveChanges();
        }
        */

        // Had to specifiy which things to update for a contact - kept getting errors for trying to update the UserId, so this is now excluded.
        public void UpdateContact(Contact contact)
        {
            using var db = _contextFactory.CreateDbContext();

            // Fetch the existing contact entity from the database, including related Persons
            var entity = db.Contacts
                          .Include(c => c.Persons)
                          .FirstOrDefault(c => c.Id == contact.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Contact with ID {contact.Id} not found.");
            }

            // Update desired properties (not UserId)
            entity.Name = contact.Name;
            entity.Address = contact.Address;
            entity.VAT = contact.VAT;
            entity.Company = contact.Company;
            entity.Type = (byte)contact.Type; // Explicit cast from enum to byte

            // Mark the updated scalar properties as modified
            db.Entry(entity).Property(c => c.Name).IsModified = true;
            db.Entry(entity).Property(c => c.Address).IsModified = true;
            db.Entry(entity).Property(c => c.VAT).IsModified = true;
            db.Entry(entity).Property(c => c.Company).IsModified = true;
            db.Entry(entity).Property(c => c.Type).IsModified = true;

            // Update related Persons
            UpdatePersons(db, entity, contact.Persons);

            // Save changes to the database
            db.SaveChanges();
        }

        private void UpdatePersons(CrmDbContext db, Persistence.Entities.Contact entity, List<Person> updatedPersons)
        {

            // Identify Persons to remove
            var personsToRemove = entity.Persons
                                        .Where(e => !updatedPersons.Any(p => p.Id == e.Id))
                                        .ToList();
            db.Persons.RemoveRange(personsToRemove);

            // Iterate through updated Persons to add or update
            foreach (var person in updatedPersons)
            {
                var existingPerson = entity.Persons.FirstOrDefault(e => e.Id == person.Id);
                if (existingPerson != null)
                {
                    // Update existing Person
                    existingPerson.Name = person.Name;
                    existingPerson.Email = person.Email;
                    existingPerson.PhoneNumber = person.Phone;
                    // EF Core will track these changes automatically
                }
                else
                {
                    // Add new Person
                    var newPerson = new Persistence.Entities.Person
                    {
                        Name = person.Name,
                        Email = person.Email,
                        PhoneNumber = person.Phone,
                        ContactId = entity.Id
                    };
                    entity.Persons.Add(newPerson);
                }
            }
        }

        public void DeleteContact(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var contact = db.Contacts
                           .Include(c => c.Persons)
                           .SingleOrDefault(c => c.Id == id);
            if (contact == default)
            {
                throw new InvalidOperationException("Contact not found");
            }
            db.Contacts.Remove(contact);
            db.SaveChanges();
        }

        public int GetCompanyIdForUser(int userId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var user = db.Users.SingleOrDefault(u => u.Id == userId);
            if (user == default)
            {
                throw new InvalidOperationException("User not found");
            }
            return user.CompanyId;
        }

        public List<ContactListRow> GetContactsForCompany(int companyId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Contacts
                     .Where(c => c.User.CompanyId == companyId)
                     .Select(c =>
                         new ContactListRow
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Type = (ContactType)c.Type,
                             Address = c.Address,
                             Company = c.Company,
                             RepName = c.User.Name,
                             VAT = c.VAT
                         })
                     .ToList();
        }
    }
}
