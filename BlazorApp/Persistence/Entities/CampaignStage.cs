namespace BlazorApp.Persistence.Entities
{
    public class CampaignStage
    {
        public int Id { get; set; }
        public int CampaignId { get; set; } // Foreign key

        public string Name { get; set; }
        public bool RequireMasterTask { get; set; }
        public string? MasterTaskDescription { get; set; }
        public bool IsConversionStage { get; set; }


        // Navigation property
        public virtual Campaign Campaign { get; set; }
    }
}
