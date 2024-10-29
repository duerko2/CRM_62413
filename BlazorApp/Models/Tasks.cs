using System;

namespace BlazorApp.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
    }
}
