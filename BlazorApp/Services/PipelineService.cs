using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services
{
    public class PipelineService
    {
        private readonly IPipelineRepository _pipelineRepository;
        private readonly ContactService _contactService;
        private readonly CampaignService _campaignService;

        public PipelineService(IPipelineRepository pipelineRepository, ContactService contactService, CampaignService campaignService)
        {
            _pipelineRepository = pipelineRepository;
            _contactService = contactService;
            _campaignService = campaignService;
        }

        public List<ContactListRow> GetContactsForUser(int userId)
        {
            return _contactService.GetContacts(userId); // Delegated to ContactService
        }

        public List<CampaignListRow> GetAllCampaigns()
        {
            return _campaignService.GetAllCampaigns(); // Delegated to CampaignService
        }

        public async Task<int> CreateNewPipeline(PipelineModel pipeline)
        {
            if (pipeline.CampaignId == 0 || pipeline.ContactId == 0)
            {
                throw new ArgumentException("Invalid Campaign or Contact selection.");
            }

            // Fetch Campaign Details
            var campaign = _campaignService.GetCampaignById(pipeline.CampaignId);
            if (campaign == null) throw new Exception("Selected campaign not found.");

            // Initialize the pipeline's active stage and master tasks
            pipeline.ActiveStage = campaign.Stages.First().Name;
            pipeline.Tasks = campaign.Stages
                .Where(s => s.RequireMasterTask)
                .Select(stage => new TaskModel
                {
                    Description = stage.MasterTaskDescription,
                    CreatedDate = DateTime.Today,
                    Deadline = DateTime.Today.AddDays(7),
                    IsMasterTask = true,
                    Stage = stage.Name,
                    IsCompleted = false
                }).ToList();

            // Add pipeline to database
            _pipelineRepository.AddPipeline(pipeline);

            return pipeline.Id;
        }


        public PipelineModel GetPipelineById(int id)
        {
            return _pipelineRepository.GetPipeline(id);
        }

        public List<PipelineListRow> GetAllPipelines()
        {
            return _pipelineRepository.GetAllPipelines();
        }

        public void AddPipeline(PipelineModel pipeline)
        {
            _pipelineRepository.AddPipeline(pipeline);
        }

        public void UpdatePipeline(PipelineModel pipeline)
        {
            _pipelineRepository.UpdatePipeline(pipeline);
        }

        public void DeletePipeline(int id)
        {
            _pipelineRepository.DeletePipeline(id);
        }

        public void AddTask(TaskModel task)
        {
            _pipelineRepository.AddTask(task);
        }

        public void UpdateTask(TaskModel task)
        {
            _pipelineRepository.UpdateTask(task);
        }

        public LatestDataModel GetLatestData(int contactId, string pipelineStage)
        {
            // For now we use hardcoded data.

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

