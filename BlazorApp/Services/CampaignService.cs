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

        public List<string> AddCampaign(CampaignModel campaign, int numberOfStages)
        {
            var errors = ValidateCampaign(campaign, numberOfStages);

            if (errors.Any())
            {
                return errors;  
            }

            _campaignRepository.AddCampaign(campaign);
            return null; // No errors
        }




        private List<string> ValidateCampaign(CampaignModel campaign, int numberOfStages)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(campaign.Name))
            {
                errors.Add("Please enter a campaign name.");
            }

            if (campaign.Stages == null || campaign.Stages.Count != numberOfStages)
            {
                errors.Add("Please define all stages.");
            }
            else
            {
                // Check if any stage names are null or whitespace
                if (campaign.Stages.Any(stage => string.IsNullOrWhiteSpace(stage.Name)))
                {
                    errors.Add("Please provide names for all stages.");
                }

                // Proceed only if no errors so far
                if (!errors.Any())
                {
                    // Can access stage.Name.Length now
                    if (campaign.Stages.Any(stage => !string.IsNullOrEmpty(stage.Name) && stage.Name.Length > 20))
                    {
                        errors.Add("Stage names must be 20 characters or fewer.");
                    }

                    if (campaign.Stages.Select(s => s.Name).Distinct().Count() != numberOfStages)
                    {
                        errors.Add("Stage names must be unique.");
                    }

                    foreach (var stage in campaign.Stages)
                    {
                        if (stage.RequireMasterTask && string.IsNullOrWhiteSpace(stage.MasterTaskDescription))
                        {
                            errors.Add($"Please provide a master task description for stage '{stage.Name}'.");
                        }
                    }
                }
            }
            int conversionStageCount = campaign.Stages.Count(s => s.IsConversionStage);
            if (conversionStageCount > 1)
            {
                errors.Add("Only one stage can be marked as the conversion stage.");
            }

            return errors;
        }
        public List<CampaignListRow> GetAllCampaigns()
        {
            return _campaignRepository.GetAllCampaigns();
        }

        public CampaignModel GetCampaignById(int id)
        {
            return _campaignRepository.GetCampaign(id);
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
