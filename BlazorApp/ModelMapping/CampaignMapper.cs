using BlazorApp.Models;
using BlazorApp.Persistence.Entities;

public static class CampaignMapper
{
    public static CampaignModel MapToModel(Campaign entity)
    {
        return new CampaignModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Stages = entity.Stages?.Select(MapStageToModel).ToList()
        };
    }

    public static CampaignStageModel MapStageToModel(CampaignStage entity)
    {
        return new CampaignStageModel
        {
            Id = entity.Id,
            Name = entity.Name,
            RequireMasterTask = entity.RequireMasterTask,
            MasterTaskDescription = entity.MasterTaskDescription,
            IsConversionStage = entity.IsConversionStage
        };
    }

    public static Campaign MapToEntity(CampaignModel model)
    {
        return new Campaign
        {
            Id = model.Id,
            Name = model.Name,
            Stages = model.Stages?.Select(MapStageToEntity).ToList()
        };
    }

    public static CampaignStage MapStageToEntity(CampaignStageModel model)
    {
        return new CampaignStage
        {
            Id = model.Id,
            Name = model.Name,
            RequireMasterTask = model.RequireMasterTask,
            MasterTaskDescription = model.MasterTaskDescription,
            IsConversionStage = model.IsConversionStage
        };
    }
}
