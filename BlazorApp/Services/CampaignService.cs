using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class CampaignService
    {
        private List<Campaign> campaigns;

        public CampaignService()
        {
            campaigns = InitializeCampaigns();
        }

        public Campaign GetCampaignById(int id)
        {
            return campaigns.FirstOrDefault(c => c.Id == id);
        }

        private List<Campaign> InitializeCampaigns()
        {
            return new List<Campaign>
            {
                new Campaign
                {
                    Id = 1,
                    Name = "Summer Promotion",
                    DefaultPipelineStages = new List<string> { "Lead", "Qualified Lead", "Proposal", "Negotiation", "Contract Sent", "Closed" },
                    StartDate = new DateTime(2024, 6, 1),
                    EndDate = new DateTime(2024, 8, 31)
                },
                new Campaign
                {
                    Id = 2,
                    Name = "Autumn Special",
                    DefaultPipelineStages = new List<string> { "Lead", "Qualified Lead", "Proposal", "Negotiation", "Closed" },
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 11, 30)
                },
                new Campaign
                {
                    Id = 3,
                    Name = "Winter Deals",
                    DefaultPipelineStages = new List<string> { "Lead", "Proposal", "Negotiation", "Contract Sent", "Closed" },
                    StartDate = new DateTime(2024, 12, 1),
                    EndDate = new DateTime(2025, 2, 28)
                },
                // Add more campaigns as needed
            };
        }
    }
}
