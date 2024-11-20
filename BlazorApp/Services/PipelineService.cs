using BlazorApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Services
{
    public class PipelineService
    {
            private List<Pipeline> pipelines = new List<Pipeline>();

            public List<Pipeline> GetAllPipelines()
            {
                return pipelines;
            }

        public List<Pipeline> GetPipelinesByContactId(int contactId)
        {
            return pipelines.Where(p => p.ContactId == contactId).ToList();
        }


        public Pipeline GetPipelineById(int id)
            {
                return pipelines.FirstOrDefault(p => p.Id == id);
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
    }
}
