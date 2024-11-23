namespace BlazorApp.Persistence.Entities
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<CampaignStage> Stages { get; set; } // Make this virtual
    }
}
