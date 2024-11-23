namespace BlazorApp.Models
{
    public class CampaignModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CampaignStageModel> Stages { get; set; } = new List<CampaignStageModel>();
    }
}