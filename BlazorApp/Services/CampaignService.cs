// Services/CampaignService.cs
using BlazorApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class CampaignService
    {
        private List<Campaign> campaigns = new List<Campaign>();

        public List<Campaign> GetAllCampaigns()
        {
            return campaigns;
        }

        public Campaign GetCampaignById(int id)
        {
            return campaigns.FirstOrDefault(c => c.Id == id);
        }

        public void AddCampaign(Campaign campaign)
        {
            campaigns.Add(campaign);
        }

        public int GetNextCampaignId()
        {
            return campaigns.Any() ? campaigns.Max(c => c.Id) + 1 : 1;
        }
    }
}
