using BlazorApp.Models;
using BlazorApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class PipelineService
    {
        private readonly IPipelineRepository _pipelineRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ContactService _contactService;
        private readonly CampaignService _campaignService;

        public PipelineService(IPipelineRepository pipelineRepository, IContactRepository contactRepository, ICampaignRepository campaignRepository, ContactService contactService, CampaignService campaignService)
        {
            _pipelineRepository = pipelineRepository;
            _contactRepository = contactRepository;
            _campaignRepository = campaignRepository;
            _contactService = contactService;
            _campaignService = campaignService;
        }

        public void UpdatePipelineStage(int pipelineId, string newStageName)
        {
            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null)
            {
                throw new Exception("Pipeline not found");
            }

            // Capture the previous stage before updating
            var previousStageName = pipeline.ActiveStage;

            // Update the active stage to the new stage
            pipeline.ActiveStage = newStageName;
            _pipelineRepository.UpdatePipeline(pipeline);

            // Fetch campaign details to access stages
            var campaign = _campaignRepository.GetCampaign(pipeline.CampaignId);
            if (campaign == null)
            {
                throw new Exception($"Campaign with ID {pipeline.CampaignId} not found.");
            }

            // Find the previous stage
            var previousStage = campaign.Stages.FirstOrDefault(s => s.Name == previousStageName);
            if (previousStage != null && previousStage.IsConversionStage)
            {
                // Convert the contact from lead to customer
                var contact = _contactRepository.GetContact(pipeline.ContactId);
                if (contact != null && contact.Type == ContactType.Lead)
                {
                    contact.Type = ContactType.Customer;
                    _contactRepository.UpdateContact(contact);
                }
            }
        }

        public List<ContactListRow> GetContactsForUser(int userId)
        {
            return _contactService.GetContacts(userId); // Delegated to ContactService
        }

        public List<CampaignListRow> GetAllCampaigns()
        {
            return _campaignService.GetAllCampaigns(); // Delegated to CampaignService
        }

        public int CreateNewPipeline(PipelineModel pipeline)
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

        public PipelineDetailModel GetPipelineDetails(int pipelineId)
        {
            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null) throw new Exception($"Pipeline with Id {pipelineId} not found.");

            var campaign = _campaignService.GetCampaignById(pipeline.CampaignId);
            var contact = _contactService.GetContactById(pipeline.ContactId);

            // Map stages with IsConversionStage flag
            var stageDetails = campaign.Stages.Select(s => new StageDetailModel
            {
                Name = s.Name,
                IsConversionStage = s.IsConversionStage
            }).ToList();

            return new PipelineDetailModel
            {
                Id = pipeline.Id,
                ActiveStage = pipeline.ActiveStage,
                Status = pipeline.Status,
                Stages = stageDetails,
                Tasks = pipeline.Tasks,
                CurrentMasterTask = pipeline.Tasks.FirstOrDefault(t => t.IsMasterTask && t.Stage == pipeline.ActiveStage && !t.IsCompleted),
                SortedTasks = pipeline.Tasks
                    .OrderBy(t => t.IsCompleted)
                    .ThenBy(t => t.IsMasterTask)
                    .ToList(),
                CampaignName = campaign.Name,
                ContactName = contact.Name,
                ContactId = contact.Id,
                LatestData = GetLatestData(contact.Id, pipeline.ActiveStage)
            };
        }

        public void ToggleStage(int pipelineId, string targetStage)
        {
            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null) throw new Exception($"Pipeline with Id {pipelineId} not found.");

            // Check if pipeline is active
            if (pipeline.Status != "Active")
            {
                throw new InvalidOperationException("Cannot change stages of a pipeline that is not active.");
            }

            var currentMasterTask = pipeline.Tasks
                .FirstOrDefault(t => t.IsMasterTask && t.Stage == pipeline.ActiveStage);

            if (currentMasterTask != null && !currentMasterTask.IsCompleted && targetStage != pipeline.ActiveStage)
            {
                throw new InvalidOperationException("Complete the master task for the current stage before proceeding.");
            }

            // Capture the previous stage before updating
            var previousStageName = pipeline.ActiveStage;

            // Update the active stage to the target stage
            pipeline.ActiveStage = targetStage;
            _pipelineRepository.UpdatePipeline(pipeline);

            // Fetch campaign details to access stages
            var campaign = _campaignRepository.GetCampaign(pipeline.CampaignId);
            if (campaign == null)
            {
                throw new Exception($"Campaign with ID {pipeline.CampaignId} not found.");
            }

            // Find the previous stage
            var previousStage = campaign.Stages.FirstOrDefault(s => s.Name == previousStageName);
            if (previousStage != null && previousStage.IsConversionStage)
            {
                // Convert the contact from lead to customer
                var contact = _contactRepository.GetContact(pipeline.ContactId);
                if (contact != null && contact.Type == ContactType.Lead)
                {
                    contact.Type = ContactType.Customer;
                    _contactRepository.UpdateContact(contact);
                }
            }
        }

        public void ToggleTaskComplete(TaskModel task)
        {
            var existingTask = _pipelineRepository.GetTaskById(task.Id);
            if (existingTask == null)
            {
                throw new Exception("Task not found.");
            }

            var pipeline = _pipelineRepository.GetPipeline(existingTask.PipelineId);
            if (pipeline == null)
            {
                throw new Exception("Pipeline not found.");
            }

            // Check if pipeline is active
            if (pipeline.Status != "Active")
            {
                throw new InvalidOperationException("Cannot modify tasks of a pipeline that is not active.");
            }

            existingTask.IsCompleted = !existingTask.IsCompleted;
            _pipelineRepository.UpdateTask(existingTask);
        }

        public void AddTask(string description, DateTime createdDate, DateTime deadline, int pipelineId)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Task description cannot be empty.");
            }

            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null) throw new Exception($"Pipeline with Id {pipelineId} not found.");

            // Check if pipeline is active
            if (pipeline.Status != "Active")
            {
                throw new InvalidOperationException("Cannot add tasks to a pipeline that is not active.");
            }

            var newTask = new TaskModel
            {
                PipelineId = pipelineId,
                Description = description,
                CreatedDate = createdDate,
                Deadline = deadline,
                IsMasterTask = false,
                IsCompleted = false,
                Stage = pipeline.ActiveStage
            };

            _pipelineRepository.AddTask(newTask);
        }

        public void EndPipelineWithWin(int pipelineId)
        {
            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null) throw new Exception($"Pipeline with Id {pipelineId} not found.");

            if (pipeline.Status != "Active")
            {
                throw new InvalidOperationException("Cannot end a pipeline that is not active.");
            }

            // Retrieve the campaign associated with the pipeline
            var campaign = _campaignService.GetCampaignById(pipeline.CampaignId);
            if (campaign == null)
            {
                throw new Exception($"Campaign with Id {pipeline.CampaignId} not found.");
            }

            // Check if the pipeline is at the last stage
            if (pipeline.ActiveStage != campaign.Stages.Last().Name)
            {
                throw new InvalidOperationException("Pipeline must be at the last stage to be ended with a win.");
            }

            // Check for incomplete master task in the last stage
            var currentMasterTask = pipeline.Tasks.FirstOrDefault(t => t.IsMasterTask && t.Stage == pipeline.ActiveStage && !t.IsCompleted);
            if (currentMasterTask != null)
            {
                throw new InvalidOperationException("Complete the master task for the last stage before ending the pipeline with a win.");
            }

            // Update pipeline status
            pipeline.Status = "Won";
            _pipelineRepository.UpdatePipeline(pipeline);
        }

        public void EndPipelineWithLoss(int pipelineId)
        {
            var pipeline = _pipelineRepository.GetPipeline(pipelineId);
            if (pipeline == null) throw new Exception($"Pipeline with Id {pipelineId} not found.");

            if (pipeline.Status != "Active")
            {
                throw new InvalidOperationException("Cannot end a pipeline that is not active.");
            }

            // Update pipeline status
            pipeline.Status = "Lost";
            _pipelineRepository.UpdatePipeline(pipeline);
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

        public List<PipelineModel> GetAllPipelinesForCampaign(int campaignId)
        {
            return _pipelineRepository.GetAllPipelinesForCampaign(campaignId);
        }
    }
}
