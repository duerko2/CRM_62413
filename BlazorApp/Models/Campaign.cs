// Models/Campaign.cs
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Stages { get; set; } = new List<string>();
        public List<bool> RequireMasterTask { get; set; } = new List<bool>();
        public List<string> MasterTaskDescriptions { get; set; } = new List<string>();
    }
}
