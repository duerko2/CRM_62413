using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public interface ICommentRepository
{ 
    int AddComment(ContactComment comment);
}