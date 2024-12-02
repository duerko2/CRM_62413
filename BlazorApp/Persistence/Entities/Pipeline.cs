namespace BlazorApp.Persistence.Entities
{
    public class Pipeline
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CampaignId { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Campaign Campaign { get; set; }
        public string ActiveStage { get; set; }
        public string Status { get; set; } = "Active"; 

        // Navigation properties
        public virtual ICollection<PipelineTask> Tasks { get; set; }
        public virtual ICollection<PipelineComment> Comments { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
