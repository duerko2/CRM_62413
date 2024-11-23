using BlazorApp.Models;
using BlazorApp.Repository;
using System.Collections.Generic;

namespace BlazorApp.Services
{
    public class CampaignService
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public List<CampaignListRow> GetAllCampaigns()
        {
            return _campaignRepository.GetAllCampaigns();
        }

        public CampaignModel GetCampaignById(int id)
        {
            return _campaignRepository.GetCampaign(id);
        }

        public void AddCampaign(CampaignModel campaign)
        {
            _campaignRepository.AddCampaign(campaign);
        }

        public void UpdateCampaign(CampaignModel campaign)
        {
            _campaignRepository.UpdateCampaign(campaign);
        }

        public void DeleteCampaign(int id)
        {
            _campaignRepository.DeleteCampaign(id);
        }
    }
}
