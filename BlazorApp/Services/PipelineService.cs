using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services
{
    public class PipelineService
    {
        private readonly IPipelineRepository _pipelineRepository;

        public PipelineService(IPipelineRepository pipelineRepository)
        {
            _pipelineRepository = pipelineRepository;
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

