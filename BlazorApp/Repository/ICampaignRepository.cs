using BlazorApp.Models;

namespace BlazorApp.Repository
{
    public interface ICampaignRepository
    {
        CampaignModel GetCampaign(int id);
        List<CampaignListRow> GetAllCampaigns();
        void AddCampaign(CampaignModel campaign);
        void UpdateCampaign(CampaignModel campaign);
        void DeleteCampaign(int id);
    }
}
