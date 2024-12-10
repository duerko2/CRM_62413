using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository
{
    public class PipelineRepository : IPipelineRepository
    {
        private readonly IDbContextFactory<CrmDbContext> _contextFactory;

        public PipelineRepository(IDbContextFactory<CrmDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public PipelineModel GetPipeline(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var pipelineEntity = db.Pipelines
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == id);

            return PipelineMapper.MapToModel(pipelineEntity);
        }

        public List<PipelineListRow> GetAllCompanyPipelines(int companyId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Pipelines
                .Include(p => p.Contact)
                .Include(p => p.Campaign)
                .Where(p => p.Contact.User.CompanyId == companyId)
                .Select(p => new PipelineListRow
                {
                    Id = p.Id,
                    ContactName = p.Contact.Name,
                    CampaignName = p.Campaign.Name,
                    Status = p.Status,
                    ActiveStage = p.ActiveStage,
                    RepName = p.Contact.User.Name
                })
                .ToList();
        }

        public List<PipelineListRow> GetAllUserPipelines(int userId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Pipelines
                .Include(p => p.Contact)
                .Include(p => p.Campaign)
                .Where(p => p.Contact.UserId == userId)
                .Select(p => new PipelineListRow
                {
                    Id = p.Id,
                    ContactName = p.Contact.Name,
                    CampaignName = p.Campaign.Name,
                    Status = p.Status,
                    ActiveStage = p.ActiveStage,
                    RepName = p.Contact.User.Name
                })
                .ToList();
        }

        public void AddPipeline(PipelineModel pipeline)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = PipelineMapper.MapToEntity(pipeline);
            db.Pipelines.Add(entity);
            db.SaveChanges();
            pipeline.Id = entity.Id;  
        }

        public void UpdatePipeline(PipelineModel pipeline)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            // Retrieve the existing entity from the database
            var existingEntity = db.Pipelines
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == pipeline.Id);

            if (existingEntity != null)
            {
                // Update properties
                existingEntity.ActiveStage = pipeline.ActiveStage;
                existingEntity.Status = pipeline.Status; 
                                                        

                // Save changes
                db.SaveChanges();
            }
            else
            {
                throw new Exception($"Pipeline with Id {pipeline.Id} not found.");
            }
        }


        public void DeletePipeline(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var pipeline = db.Pipelines.Find(id);
            if (pipeline != null)
            {
                db.Pipelines.Remove(pipeline);
                db.SaveChanges();
            }
        }

        public void AddTask(TaskModel task)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = TaskMapper.MapToEntity(task);
            db.PipelineTasks.Add(entity);
            db.SaveChanges();
            task.Id = entity.Id;  
        }

        public void UpdateTask(TaskModel task)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var existingEntity = db.PipelineTasks.FirstOrDefault(t => t.Id == task.Id);

            if (existingEntity != null)
            {
                TaskMapper.MapToExistingEntity(task, existingEntity);
                db.SaveChanges();
            }
            else
            {
                throw new Exception($"Task with Id {task.Id} not found.");
            }
        }

        public TaskModel GetTaskById(int taskId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var taskEntity = db.PipelineTasks.FirstOrDefault(t => t.Id == taskId);
            return TaskMapper.MapToModel(taskEntity);
        }
        public List<PipelineModel> GetAllPipelinesForCampaign(int campaignId)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Pipelines
                .Where(p => p.CampaignId == campaignId)
                .Select(p => new PipelineModel
                {
                    Id = p.Id,
                    ContactId = p.ContactId,
                    CampaignId = p.CampaignId,
                    ActiveStage = p.ActiveStage,
                    Status = p.Status,
                    Tasks = p.Tasks.Select(t => new TaskModel
                    {
                        Id = t.Id,
                        PipelineId = t.PipelineId,
                        Description = t.Description,
                        Deadline = t.Deadline,
                        CreatedDate = t.CreatedDate,
                        IsCompleted = t.IsCompleted,
                    }).ToList()
                })
                .ToList();
        }
    }
}
