using System;
using System.Collections.Generic;

namespace BlazorApp.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> DefaultPipelineStages { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
