using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public class EfCommentRepository : ICommentRepository
{
    private readonly CrmDbContext _db;

    public EfCommentRepository(CrmDbContext db)
    {
        _db = db;
    }
    
    public int AddComment(ContactComment comment)
    {
        _db.ContactComments.Add(comment);
        _db.SaveChanges();
        return comment.Id;
    }
}