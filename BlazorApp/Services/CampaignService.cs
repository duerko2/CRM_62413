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
            // Initialize campaigns with sample data
            campaigns = InitializeCampaigns();
        }

        // Get a campaign by its ID
        public Campaign GetCampaignById(int id)
        {
            return campaigns.FirstOrDefault(c => c.Id == id);
        }

        // Get all campaigns
        public List<Campaign> GetAllCampaigns()
        {
            return campaigns;
        }

        // Initialize sample campaigns
        private List<Campaign> InitializeCampaigns()
        {
            return new List<Campaign>
            {
                new Campaign
                {
                    Id = 1,
                    Name = "Summer Promotion",
                    ActiveStage = "Lead",
                    PipelineStages = new List<string> { "Lead", "Qualified Lead", "Proposal", "Negotiation", "Contract Sent", "Closed" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = 1,
                            Description = "Send lead updated proposal",
                            CreatedDate = new DateTime(2024, 9, 10),
                            Deadline = new DateTime(2024, 9, 15)
                        },
                        new TaskModel
                        {
                            Id = 2,
                            Description = "Follow up on contract",
                            CreatedDate = new DateTime(2024, 9, 12),
                            Deadline = new DateTime(2024, 9, 18)
                        }
                    }
                },
                new Campaign
                {
                    Id = 2,
                    Name = "Winter Sale",
                    ActiveStage = "Qualified Lead",
                    PipelineStages = new List<string> { "Lead", "Qualified Lead", "Presentation", "Proposal", "Negotiation", "Closed" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = 3,
                            Description = "Prepare presentation slides",
                            CreatedDate = new DateTime(2024, 11, 1),
                            Deadline = new DateTime(2024, 11, 5)
                        },
                        new TaskModel
                        {
                            Id = 4,
                            Description = "Schedule meeting with client",
                            CreatedDate = new DateTime(2024, 11, 2),
                            Deadline = new DateTime(2024, 11, 6)
                        }
                    }
                },
                new Campaign
                {
                    Id = 3,
                    Name = "Spring Launch",
                    ActiveStage = "Proposal",
                    PipelineStages = new List<string> { "Awareness", "Interest", "Decision", "Action" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = 5,
                            Description = "Draft initial proposal",
                            CreatedDate = new DateTime(2024, 3, 15),
                            Deadline = new DateTime(2024, 3, 20)
                        },
                        new TaskModel
                        {
                            Id = 6,
                            Description = "Review proposal with team",
                            CreatedDate = new DateTime(2024, 3, 16),
                            Deadline = new DateTime(2024, 3, 22)
                        }
                    }
                },
                new Campaign
                {
                    Id = 4,
                    Name = "Autumn Deals",
                    ActiveStage = "Negotiation",
                    PipelineStages = new List<string> { "Initial Contact", "Demonstration", "Proposal", "Negotiation", "Close" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = 7,
                            Description = "Conduct product demonstration",
                            CreatedDate = new DateTime(2024, 9, 5),
                            Deadline = new DateTime(2024, 9, 10)
                        },
                        new TaskModel
                        {
                            Id = 8,
                            Description = "Negotiate contract terms",
                            CreatedDate = new DateTime(2024, 9, 12),
                            Deadline = new DateTime(2024, 9, 20)
                        }
                    }
                }
            };
        }
        public void SetActiveStage(Campaign campaign, string stage)
        {
            campaign.ActiveStage = stage;
        }

        public LatestDataModel GetLatestData(int contactId, string pipelineStage)
        {
            // Simulated logic for fetching latest data
            if (pipelineStage == "Proposal")
            {
                return new LatestDataModel
                {
                    LatestType = "Quote",
                    Details = "Proposal for client with updated pricing",
                    DateCreated = new DateTime(2024, 9, 15)
                };
            }
            else if (pipelineStage == "Contract Sent" || pipelineStage == "Negotiation")
            {
                return new LatestDataModel
                {
                    LatestType = "Order",
                    Details = "Order placed for contract #12345",
                    DateCreated = new DateTime(2024, 10, 1)
                };
            }
            else
            {
                return new LatestDataModel
                {
                    Message = "No latest quote, order, or invoice available."
                };
            }
        }
    }
}
