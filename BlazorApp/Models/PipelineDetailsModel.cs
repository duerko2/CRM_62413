namespace BlazorApp.Models
{
    public class PipelineDetailModel
    {
        public int Id { get; set; }
        public string ActiveStage { get; set; }
        public List<StageDetailModel> Stages { get; set; } // Updated to StageDetailModel
        public List<TaskModel> Tasks { get; set; }
        public TaskModel CurrentMasterTask { get; set; }
        public List<TaskModel> SortedTasks { get; set; }
        public string CampaignName { get; set; }
        public string ContactName { get; set; }
        public int ContactId { get; set; }
        public LatestDataModel LatestData { get; set; }
        public string Status { get; set; }

    }
}
