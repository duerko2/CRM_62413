using BlazorApp.Components.Pages;
using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services;

public class DashboardService
{
    IDashboardRepository _dashboardRepository;
    CampaignService _campaignService;
    PipelineService _pipelineService;
    
    public DashboardService(IDashboardRepository dashboardRepository, CampaignService campaignService, PipelineService pipelineService)
    {
        _dashboardRepository = dashboardRepository;
        _campaignService = campaignService;
        _pipelineService = pipelineService;
    }

    public List<AvailableCampaigns> GetAvailableCampaigns(int userContextUserId)
    {
        return _campaignService.GetAllCampaigns()
            .Select(campaign => 
                new AvailableCampaigns
                {
                    Id = campaign.Id, 
                    Name = campaign.Name
                })
            .ToList();
    }

    public FunnelView GetCampaignFunnel(int campaignId, int userId)
    {
        FunnelView funnelView = new FunnelView();

        var stages = _pipelineService.GetAllPipelinesForCampaign(campaignId).GroupBy(p => new {p.Id, p.ActiveStage})
            .Select(g => new FunnelViewStage
            {
                Id = g.Key.Id,
                StageName = g.Key.ActiveStage,
                Count = g.Count(),
            })
            .OrderBy(p => p.Id)
            .Reverse()
            .ToList();
        foreach (var funnelViewStage in stages)
        {
            Console.WriteLine(funnelViewStage.StageName);
        }
        
        funnelView.Stages = stages;
        return funnelView;
    }

    public FunnelView GetCampaignFunnel(int campaignId)
    {
        FunnelView funnelView = new FunnelView();

        var stages = _pipelineService.GetAllPipelinesForCampaign(campaignId)
            .GroupBy(p => p.ActiveStage)
            .Select(g => new FunnelViewStage
            {
                StageName = g.Key,
                Count = g.Count(),
            })
            .Reverse()
            .ToList();
        
        foreach (var funnelViewStage in stages)
        {
            Console.WriteLine(funnelViewStage.StageName);
        }

        
        funnelView.Stages = stages;
        return funnelView;
    }
}
