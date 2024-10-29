using System;
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Changed from 'CampaignName' to 'Name' for consistency
        public string ActiveStage { get; set; }
        public List<string> PipelineStages { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public DateTime? StartDate { get; set; }  
        public DateTime? EndDate { get; set; }    
    }
}
