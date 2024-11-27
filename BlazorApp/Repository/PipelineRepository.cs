using BlazorApp.ModelMapping;
using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository
{
    public class PipelineRepository : IPipelineRepository
    {
        private readonly CrmDbContext _dbContext;

        public PipelineRepository(CrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PipelineModel GetPipeline(int id)
        {
            var pipelineEntity = _dbContext.Pipelines
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == id);

            return PipelineMapper.MapToModel(pipelineEntity);
        }

        public List<PipelineListRow> GetAllPipelines()
        {
            return _dbContext.Pipelines
                .Include(p => p.Contact)
                .Include(p => p.Campaign)
                .Select(p => new PipelineListRow
                {
                    Id = p.Id,
                    ContactName = p.Contact.Name,
                    CampaignName = p.Campaign.Name,
                    ActiveStage = p.ActiveStage
                })
                .ToList();
        }

        public void AddPipeline(PipelineModel pipeline)
        {
            var entity = PipelineMapper.MapToEntity(pipeline);
            _dbContext.Pipelines.Add(entity);
            _dbContext.SaveChanges();
            pipeline.Id = entity.Id; // Update the model with the generated ID
        }

        public void UpdatePipeline(PipelineModel pipeline)
        {
            // Retrieve the existing entity from the database
            var existingEntity = _dbContext.Pipelines
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == pipeline.Id);

            if (existingEntity != null)
            {
                // Update scalar properties
                existingEntity.ActiveStage = pipeline.ActiveStage;
                // Update other properties as needed

                // Save changes
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception($"Pipeline with Id {pipeline.Id} not found.");
            }
        }

        public void DeletePipeline(int id)
        {
            var pipeline = _dbContext.Pipelines.Find(id);
            if (pipeline != null)
            {
                _dbContext.Pipelines.Remove(pipeline);
                _dbContext.SaveChanges();
            }
        }

        public void AddTask(TaskModel task)
        {
            var entity = TaskMapper.MapToEntity(task);
            _dbContext.PipelineTasks.Add(entity);
            _dbContext.SaveChanges();
            task.Id = entity.Id; // Update task ID
        }

        public void UpdateTask(TaskModel task)
        {
            var existingEntity = _dbContext.PipelineTasks.FirstOrDefault(t => t.Id == task.Id);

            if (existingEntity != null)
            {
                TaskMapper.MapToExistingEntity(task, existingEntity);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception($"Task with Id {task.Id} not found.");
            }
        }
    }
}
