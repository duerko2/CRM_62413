namespace BlazorApp.Persistence.Entities;

public class PipelineComment
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public int PipelineId { get; set; }
    public virtual Pipeline Pipeline { get; set; }
}