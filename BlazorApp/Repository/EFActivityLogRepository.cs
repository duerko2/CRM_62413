using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public class EFActivityLogRepository : IActivityLogRepository
{
    private readonly CrmDbContext _db;
    
    public EFActivityLogRepository(CrmDbContext db)
    {
        _db = db;
    }

    public List<ActivityLog> GetContactActivityLogs(int contactId)
    {
        return _db.ActivityLogs.Where(a => a.ContactId == contactId).ToList();
    }

    public void AddActivityLog(ActivityLog activityLog)
    {
        _db.ActivityLogs.Add(activityLog);
        _db.SaveChanges();
    }
}