namespace BlazorApp.Persistence.Entities
{
    public class PipelineTask
    {
        public int Id { get; set; }
        public int PipelineId { get; set; }
        public virtual Pipeline Pipeline { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsMasterTask { get; set; }
        public string Stage { get; set; }
        public bool IsCompleted { get; set; }
    }
}
