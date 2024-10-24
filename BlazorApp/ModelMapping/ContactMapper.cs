using BlazorApp.Models;

namespace BlazorApp.ModelMapping;

public class ContactMapper
{
    public static Persistence.Entities.Contact MapToEntity(Contact model)
    {
        var contactEntity = new Persistence.Entities.Contact
        {
            Id = model.Id,
            Name = model.Name,
            Address = model.Address,
            UserId = 2, // Hardcoded for now
        };
        var persons = MapPersonsToEntity(contactEntity.Id, model.Persons);
        contactEntity.Persons = persons;
        
        return contactEntity;
    }

    public static Contact MapToModel(Persistence.Entities.Contact entity)
    {
        var contactModel = new Contact
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address,
        };
        var persons = MapPersonsToModel(entity.Persons);
        contactModel.Persons = (List<Person>)persons;
        return contactModel;
    }

    private static object MapPersonsToModel(ICollection<Persistence.Entities.Person> entityPersons)
    {
        return entityPersons.Select(p => new Person
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            Phone = p.PhoneNumber
        }).ToList();
    }

    private static List<Persistence.Entities.Person> MapPersonsToEntity(int contactId, IEnumerable<Person> persons)
    {
        return persons.Select(p => new Persistence.Entities.Person
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            PhoneNumber = p.Phone,
            ContactId = contactId
        }).ToList();
    }
}