namespace BlazorApp.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CampaignName { get; set; }
        public string ContactName { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public List<string> PipelineStages { get; set; }
        public string ActiveStage { get; set; }
    }
}
