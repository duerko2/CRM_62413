using System;
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Pipeline
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CampaignId { get; set; }
        public string ActiveStage { get; set; }
        public List<string> PipelineStages { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
