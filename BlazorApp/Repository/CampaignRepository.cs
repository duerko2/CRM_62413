using BlazorApp.Models;
using BlazorApp.Persistence;
using BlazorApp.Persistence.Entities;
using BlazorApp.ModelMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BlazorApp.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IDbContextFactory<CrmDbContext> _contextFactory;

        public CampaignRepository(IDbContextFactory<CrmDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public CampaignModel GetCampaign(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var campaignEntity = db.Campaigns
                .Include(c => c.Stages)
                .FirstOrDefault(c => c.Id == id);

            if (campaignEntity == null)
                return null;

            return CampaignMapper.MapToModel(campaignEntity);
        }

        public List<CampaignListRow> GetAllCampaigns()
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            return db.Campaigns
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
            using CrmDbContext db = _contextFactory.CreateDbContext();

            var entity = CampaignMapper.MapToEntity(campaign);
            db.Campaigns.Add(entity);
            db.SaveChanges();
            campaign.Id = entity.Id; 
        }

        public void UpdateCampaign(CampaignModel campaign)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var entity = CampaignMapper.MapToEntity(campaign);
            db.Campaigns.Update(entity);
            db.SaveChanges();
        }

        public void DeleteCampaign(int id)
        {
            using CrmDbContext db = _contextFactory.CreateDbContext();
            var campaign = db.Campaigns.Find(id);
            if (campaign != null)
            {
                db.Campaigns.Remove(campaign);
                db.SaveChanges();
            }
        }
    }
}
