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

        public List<PipelineListRow> GetAllPipelines()
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Pipelines
                .Include(p => p.Contact)
                .Include(p => p.Campaign)
                .Select(p => new PipelineListRow
                {
                    Id = p.Id,
                    ContactName = p.Contact.Name,
                    CampaignName = p.Campaign.Name,
                    Status = p.Status,
                    ActiveStage = p.ActiveStage
                })
                .ToList();
        }

        public void AddPipeline(PipelineModel pipeline)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = PipelineMapper.MapToEntity(pipeline);
            db.Pipelines.Add(entity);
            db.SaveChanges();
            pipeline.Id = entity.Id; // Update the model with the generated ID
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
            task.Id = entity.Id; // Update task ID
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
    }
}
