namespace BlazorApp.Models
{
    public class PipelineModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CampaignId { get; set; }
        public string ActiveStage { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public string Status { get; set; } = "Active"; 

    }
}
