using BlazorApp.Models;
using BlazorApp.Persistence.Entities;

namespace BlazorApp.ModelMapping
{
    public static class TaskMapper
    {
        public static TaskModel MapToModel(Persistence.Entities.PipelineTask entity)
        {
            if (entity == null)
                return null;

            return new TaskModel
            {
                Id = entity.Id,
                PipelineId = entity.PipelineId,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                Deadline = entity.Deadline,
                IsMasterTask = entity.IsMasterTask,
                Stage = entity.Stage,
                IsCompleted = entity.IsCompleted
            };
        }

        public static Persistence.Entities.PipelineTask MapToEntity(TaskModel model)
        {
            if (model == null)
                return null;

            return new Persistence.Entities.PipelineTask
            {
                Id = model.Id,
                PipelineId = model.PipelineId,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                Deadline = model.Deadline,
                IsMasterTask = model.IsMasterTask,
                Stage = model.Stage,
                IsCompleted = model.IsCompleted
            };
        }
        public static void MapToExistingEntity(TaskModel model, PipelineTask entity)
        {
            if (model == null || entity == null)
                return;

            entity.Description = model.Description;
            entity.CreatedDate = model.CreatedDate;
            entity.Deadline = model.Deadline;
            entity.IsMasterTask = model.IsMasterTask;
            entity.Stage = model.Stage;
            entity.IsCompleted = model.IsCompleted;

        }
    }
}
