namespace BlazorApp.Models;

public static class ActivityLogText
{
    static Dictionary<ActivityLogType,string> _activityLogTexts = new()
    {
        {0,null},
        { ActivityLogType.ContactCreated, "Contact created." },
        { ActivityLogType.ContactUpdated, "Contact updated." }
    };
    
    public static string GetActivityLogText(ActivityLogType type)
    {
        return string.Format(_activityLogTexts[type]);
    }
}