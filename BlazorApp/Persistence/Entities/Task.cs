namespace BlazorApp.Persistence.Entities;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string CreatedDate { get; set; }
    public string Deadline { get; set; }
    public int PipelineId { get; set; }
    public virtual Pipeline Pipeline { get; set; }
}