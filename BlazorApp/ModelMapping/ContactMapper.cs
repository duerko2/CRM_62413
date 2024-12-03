using BlazorApp.Models;
using BlazorApp.Persistence.Entities;
using Contact = BlazorApp.Models.Contact;
using ContactComment = BlazorApp.Models.ContactComment;
using Person = BlazorApp.Models.Person;

namespace BlazorApp.ModelMapping;

public class ContactMapper
{
    public static Persistence.Entities.Contact MapToEntity(Contact model, int userId)
    {
        var contactEntity = new Persistence.Entities.Contact
        {
            Id = model.Id,
            Name = model.Name,
            Address = model.Address,
            UserId = userId,
            VAT = model.VAT,
            Company = model.Company,
            Type = (byte) model.Type
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
            VAT = entity.VAT,
            Company = entity.Company,
            Type = (ContactType) entity.Type
        };
        contactModel.Persons = MapPersonsToModel(entity.Persons);
        contactModel.Pipelines = MapPipelinesToModel(entity);
        contactModel.Comments = MapCommentsToModel(entity, contactModel);
        contactModel.Activities = MapActivitiesToModel(entity.ActivityLogs);
        
        contactModel.Activities.AddRange(contactModel.Comments.Select(a => new Activity
        {
            Date = a.Date,
            Description = $"Comment added: '{a.Text}'",
        }).ToList());
        
        return contactModel;
    }

    private static List<Activity> MapActivitiesToModel(ICollection<ActivityLog> entityActivityLogs)
    {
        return entityActivityLogs.Select(a => new Activity
        {
            Date = a.Date,
            Description = ActivityLogText.GetActivityLogText((ActivityLogType) a.Type)
        }).ToList();
    }

    private static List<ContactComment> MapCommentsToModel(Persistence.Entities.Contact entity, Contact contactModel)
    {
        return entity.Comments.Select(c => new ContactComment
        {
            Id = c.Id,
            Text = c.Text,
            Date = c.Date,
            ContactId = c.ContactId,
            Contact = contactModel
        }).ToList();
    }

    private static List<PipelineModel> MapPipelinesToModel(Persistence.Entities.Contact entity)
    {
        if (entity.Pipelines == null)
        {
            return new List<PipelineModel>();
        }

        return entity.Pipelines.Select(p => new PipelineModel
        {
            Id = p.Id,
            CampaignId = p.CampaignId,
            ContactId = p.ContactId,
            ActiveStage = p.ActiveStage,
            Status = p.Status,
            Tasks = p.Tasks != null ? p.Tasks.Select(TaskMapper.MapToModel).ToList() : new List<TaskModel>()
        }).ToList();
    }


    private static List<Person> MapPersonsToModel(ICollection<Persistence.Entities.Person> entityPersons)
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