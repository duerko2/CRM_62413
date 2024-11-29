namespace BlazorApp.Models;

public class TaskViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string PipelineName { get; set; }
    public int PipelineId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsOverdue { get; set; }
    public bool IsCloseToDeadline { get; set; }
}
