namespace BlazorApp.Persistence.Entities;

public class Pipeline
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; }
    public int CampaignId { get; set; }
    public virtual Campaign Campaign { get; set; }
    
    // Navigation property to Task
    public virtual ICollection<Task> Tasks { get; set; }
}