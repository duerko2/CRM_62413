using BlazorApp.Models;
using System.Collections.Generic;

namespace BlazorApp.Services
{
    public class CampaignService
    {
        // Get campaign data (for now hardcoded, will be dynamic in the future)
        public CampaignModel GetCampaignData()
        {
            return new CampaignModel
            {
                CampaignName = "Campaign_X",
                ContactName = "Contact_Y",
                ActiveStage = "Lead",
                PipelineStages = new List<string> { "Lead", "Qualified lead", "Proposal", "Negotiation", "Contract sent", "Closed" },
                Tasks = new List<TaskModel>
                {
                    new TaskModel { Description = "Send lead updated proposal", CreatedDate = "10/09/2024", Deadline = "15/09/2024" },
                    new TaskModel { Description = "Follow up on contract", CreatedDate = "12/09/2024", Deadline = "18/09/2024" }
                }
            };
        }

        // Set the active stage of the campaign
        public void SetActiveStage(CampaignModel campaign, string stage)
        {
            campaign.ActiveStage = stage; // Update the active stage in the model
        }

        // Get the latest quote, order, or invoice (for now hardcoded, will be fetched from database in the future)
        public LatestDataModel GetLatestData(int contactId, string pipelineStage)
        {
            // Simulate the logic for fetching the latest data
            // In the future, this will be dynamically fetched from the database

            if (pipelineStage == "Proposal")
            {
                return new LatestDataModel
                {
                    LatestType = "Quote",
                    Details = "Proposal for client with updated pricing",
                    DateCreated = new System.DateTime(2024, 9, 15)
                };
            }
            else if (pipelineStage == "Contract sent")
            {
                return new LatestDataModel
                {
                    LatestType = "Order",
                    Details = "Order placed for contract #12345",
                    DateCreated = new System.DateTime(2024, 10, 1)
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
