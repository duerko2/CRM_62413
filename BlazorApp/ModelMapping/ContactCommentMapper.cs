using BlazorApp.Models;

namespace BlazorApp.Services;

public class ContactCommentMapper
{
    public static Persistence.Entities.ContactComment MapToEntity(ContactComment comment)
    {
        return new Persistence.Entities.ContactComment
        {
            Id = comment.Id,
            Text = comment.Text,
            ContactId = comment.ContactId,
            Date = comment.Date
        };
    }
}