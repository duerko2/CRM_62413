namespace BlazorApp.Models
{
    public class CampaignStageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RequireMasterTask { get; set; }
        public string MasterTaskDescription { get; set; }
    }
}