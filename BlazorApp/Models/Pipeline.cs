// Models/Pipeline.cs
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Pipeline
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CampaignId { get; set; }
        public List<string> PipelineStages { get; set; } = new List<string>();
        public List<bool> RequireMasterTask { get; set; } = new List<bool>();
        public List<string> MasterTaskDescriptions { get; set; } = new List<string>();
        public string ActiveStage { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
