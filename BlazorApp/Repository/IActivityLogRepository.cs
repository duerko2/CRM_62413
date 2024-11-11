using BlazorApp.Persistence.Entities;

namespace BlazorApp.Repository;

public interface IActivityLogRepository
{
    List<ActivityLog> GetContactActivityLogs(int contactId);
    void AddActivityLog(ActivityLog activityLog);
}