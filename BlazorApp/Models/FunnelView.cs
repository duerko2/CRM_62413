namespace BlazorApp.Models;

public class FunnelView
{
    public string CampaignName { get; set; }
    public List<FunnelViewStage> Stages { get; set; }
}

public class FunnelViewStage
{
    public int Count { get; set; }
    public string StageName { get; set; } // Optional: Add stage names for better context.
}
