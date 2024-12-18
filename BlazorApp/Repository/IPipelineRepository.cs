﻿using BlazorApp.Models;

namespace BlazorApp.Repository
{
    public interface IPipelineRepository
    {
        PipelineModel GetPipeline(int id);
        List<PipelineListRow> GetAllCompanyPipelines(int companyId);
        List<PipelineListRow> GetAllUserPipelines(int userId);
        void AddPipeline(PipelineModel pipeline);
        void UpdatePipeline(PipelineModel pipeline);
        void DeletePipeline(int id);

        void AddTask(TaskModel task);
        void UpdateTask(TaskModel task);
        TaskModel GetTaskById(int taskId);
        List<PipelineModel> GetAllPipelinesForCampaign(int campaignId);
    }
}
