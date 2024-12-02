using BlazorApp.Models;

namespace BlazorApp.ModelMapping
{
    public static class PipelineMapper
    {
        public static PipelineModel MapToModel(Persistence.Entities.Pipeline entity)
        {
            if (entity == null)
                return null;

            return new PipelineModel
            {
                Id = entity.Id,
                ContactId = entity.ContactId,
                CampaignId = entity.CampaignId,
                ActiveStage = entity.ActiveStage,
                Status = entity.Status,
                Tasks = entity.Tasks?.Select(TaskMapper.MapToModel).ToList() ?? new List<TaskModel>()
            };
        }

        public static Persistence.Entities.Pipeline MapToEntity(PipelineModel model)
        {
            if (model == null)
                return null;

            return new Persistence.Entities.Pipeline
            {
                Id = model.Id,
                ContactId = model.ContactId,
                CampaignId = model.CampaignId,
                ActiveStage = model.ActiveStage,
                Status = model.Status,
                Tasks = model.Tasks?.Select(TaskMapper.MapToEntity).ToList() ?? new List<Persistence.Entities.PipelineTask>()
            };
        }
    }
}
