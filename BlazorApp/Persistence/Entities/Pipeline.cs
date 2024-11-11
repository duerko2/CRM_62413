namespace BlazorApp.Persistence.Entities;

public class Pipeline
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; }
    public int CampaignId { get; set; }
    // Navigation properties

    public virtual Campaign Campaign { get; set; }
    public virtual ICollection<Task> Tasks { get; set; }
    public virtual ICollection<PipelineComment> Comments { get; set; }
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
}