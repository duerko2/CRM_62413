using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using BlazorApp.ModelMapping;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly CrmDbContext _dbContext;

        public CampaignRepository(CrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CampaignModel GetCampaign(int id)
        {
            var campaignEntity = _dbContext.Campaigns
                .Include(c => c.Stages)
                .FirstOrDefault(c => c.Id == id);

            if (campaignEntity == null)
                return null;

            return CampaignMapper.MapToModel(campaignEntity);
        }

        public List<CampaignListRow> GetAllCampaigns()
        {
            return _dbContext.Campaigns
                .Include(c => c.Stages)
                .Select(c => new CampaignListRow
                {
                    Id = c.Id,
                    Name = c.Name,
                    NumberOfStages = c.Stages.Count,
                    NumberOfPipelines = c.Pipelines.Count
                })
                .ToList();
        }

        public void AddCampaign(CampaignModel campaign)
        {
            var entity = CampaignMapper.MapToEntity(campaign);
            _dbContext.Campaigns.Add(entity);
            _dbContext.SaveChanges();
            campaign.Id = entity.Id; // Update the model with the generated ID
        }

        public void UpdateCampaign(CampaignModel campaign)
        {
            var entity = CampaignMapper.MapToEntity(campaign);
            _dbContext.Campaigns.Update(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteCampaign(int id)
        {
            var campaign = _dbContext.Campaigns.Find(id);
            if (campaign != null)
            {
                _dbContext.Campaigns.Remove(campaign);
                _dbContext.SaveChanges();
            }
        }
    }
}
