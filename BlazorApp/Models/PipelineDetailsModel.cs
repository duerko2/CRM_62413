﻿namespace BlazorApp.Models
{
    public class PipelineDetailModel
    {
        public int Id { get; set; }
        public string ActiveStage { get; set; }
        public List<string> Stages { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public TaskModel CurrentMasterTask { get; set; }
        public List<TaskModel> SortedTasks { get; set; }
        public string CampaignName { get; set; }
        public string ContactName { get; set; }
        public int ContactId { get; set; }
        public LatestDataModel LatestData { get; set; }
    }
}