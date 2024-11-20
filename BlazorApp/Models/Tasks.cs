// Models/TaskModel.cs
using System;

namespace BlazorApp.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int PipelineId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsMasterTask { get; set; }
        public string Stage { get; set; }
    }
}
