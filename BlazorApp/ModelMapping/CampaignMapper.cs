using BlazorApp.Models;
using BlazorApp.Persistence.Entities;
using System.Linq;

namespace BlazorApp.ModelMapping
{
    public static class CampaignMapper
    {
        public static CampaignModel MapToModel(Campaign entity)
        {
            return new CampaignModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Stages = entity.Stages.Select(MapStageToModel).ToList()
            };
        }

        public static CampaignStageModel MapStageToModel(CampaignStage entity)
        {
            return new CampaignStageModel
            {
                Id = entity.Id,
                Name = entity.Name,
                RequireMasterTask = entity.RequireMasterTask,
                MasterTaskDescription = entity.MasterTaskDescription
            };
        }

        public static Campaign MapToEntity(CampaignModel model)
        {
            return new Campaign
            {
                Id = model.Id,
                Name = model.Name,
                Stages = model.Stages.Select(MapStageToEntity).ToList()
            };
        }

        public static CampaignStage MapStageToEntity(CampaignStageModel model)
        {
            return new CampaignStage
            {
                Id = model.Id,
                Name = model.Name,
                RequireMasterTask = model.RequireMasterTask,
                MasterTaskDescription = model.MasterTaskDescription
            };
        }
    }
}
