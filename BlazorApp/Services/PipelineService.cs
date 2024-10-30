using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class PipelineService
    {
        private List<Pipeline> pipelines;
        private CampaignService campaignService;

        public PipelineService(CampaignService campaignService)
        {
            this.campaignService = campaignService;
            pipelines = InitializePipelines();
        }

        public List<Pipeline> GetAllPipelines()
        {
            return pipelines;
        }

        public Pipeline GetPipelineById(int id)
        {
            return pipelines.FirstOrDefault(p => p.Id == id);
        }

        public List<Pipeline> GetPipelinesByContactId(int contactId)
        {
            return pipelines.Where(p => p.ContactId == contactId).ToList();
        }

        public void AddPipeline(Pipeline pipeline)
        {
            pipelines.Add(pipeline);
        }

        public int GetNextPipelineId()
        {
            return pipelines.Any() ? pipelines.Max(p => p.Id) + 1 : 1;
        }

        // Simulate fetching latest data
        public LatestDataModel GetLatestData(int contactId, string pipelineStage)
        {
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

        private List<Pipeline> InitializePipelines()
        {
            var pipelinesList = new List<Pipeline>();
            int pipelineIdCounter = 1;

            for (int contactId = 1; contactId <= 10; contactId++)
            {
                // First Pipeline for each contact
                pipelinesList.Add(new Pipeline
                {
                    Id = pipelineIdCounter++,
                    ContactId = contactId,
                    CampaignId = (contactId % 3) + 1, // Cycle through CampaignIds 1, 2, 3
                    ActiveStage = "Lead",
                    PipelineStages = new List<string> { "Lead", "Qualified Lead", "Proposal", "Negotiation", "Contract Sent", "Closed" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = pipelineIdCounter * 10 + 1,
                            PipelineId = pipelineIdCounter - 1,
                            Description = $"Initial contact with contact {contactId}",
                            CreatedDate = DateTime.Now.AddDays(-contactId),
                            Deadline = DateTime.Now.AddDays(30 - contactId)
                        }
                    },
                    StartDate = DateTime.Now.AddDays(-contactId),
                    EndDate = DateTime.Now.AddMonths(1)
                });

                // Second Pipeline for each contact
                pipelinesList.Add(new Pipeline
                {
                    Id = pipelineIdCounter++,
                    ContactId = contactId,
                    CampaignId = ((contactId + 1) % 3) + 1, // Cycle through CampaignIds 1, 2, 3
                    ActiveStage = "Proposal",
                    PipelineStages = new List<string> { "Lead", "Proposal", "Negotiation", "Closed" },
                    Tasks = new List<TaskModel>
                    {
                        new TaskModel
                        {
                            Id = pipelineIdCounter * 10 + 2,
                            PipelineId = pipelineIdCounter - 1,
                            Description = $"Prepare proposal for contact {contactId}",
                            CreatedDate = DateTime.Now.AddDays(-contactId + 1),
                            Deadline = DateTime.Now.AddDays(30 - contactId + 1)
                        }
                    },
                    StartDate = DateTime.Now.AddDays(-contactId + 1),
                    EndDate = DateTime.Now.AddMonths(2)
                });
            }

            return pipelinesList;
        }
    }
}
