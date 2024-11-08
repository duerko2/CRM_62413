using System.Data;

namespace BlazorApp.Persistence.Entities;

public class Campaign
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    
    // Navigation property to Pipeline
    public virtual ICollection<Pipeline> Pipelines { get; set; }
}