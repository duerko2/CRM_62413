namespace BlazorApp.Persistence.Entities;

public class ActivityLog
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int? ContactId { get; set; }
    public int? PipelineId { get; set; }
    public int Type { get; set; }
    public virtual Contact Contact { get; set; }
    public virtual Pipeline Pipeline { get; set; }
}