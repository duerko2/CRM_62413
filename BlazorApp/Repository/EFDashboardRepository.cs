using BlazorApp.Models;
using BlazorApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Repository;

public class EFDashboardRepository : IDashboardRepository
{
    private readonly IDbContextFactory<CrmDbContext> _contextFactory;

    public EFDashboardRepository(IDbContextFactory<CrmDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public List<TaskViewModel> GetTasksForUser(int userContextUserId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        return db.Tasks
            .Where(t => t.Pipeline.Contact.UserId == userContextUserId)
            .Select(t => new TaskViewModel
            {
                Id = t.Id,
                Description = t.Description,
                PipelineName = t.Pipeline.Campaign.Name + " - " + t.Pipeline.Contact.Name,
                PipelineId = t.PipelineId,
                CreatedDate = t.CreatedDate,
                Deadline = t.Deadline,
                IsOverdue = t.Deadline < DateTime.Now && !t.IsCompleted,
                IsCloseToDeadline = t.Deadline < DateTime.Now.AddDays(3) && !t.IsCompleted
            })
            .ToList();
    }

    public List<TaskViewModel> GetTasksForCompany(int userContextUserId)
    {
        using CrmDbContext db = _contextFactory.CreateDbContext();
        var companyId = db.Users.SingleOrDefault(u => u.Id == userContextUserId)?.CompanyId ?? throw new Exception("Company not found");
        
        return db.Tasks
            .Where(t => t.Pipeline.Contact.User.CompanyId == companyId)
            .Select(t => new TaskViewModel
            {
                Id = t.Id,
                Description = t.Description,
                PipelineName = t.Pipeline.Campaign.Name + " - " + t.Pipeline.Contact.Name,
                PipelineId = t.PipelineId,
                CreatedDate = t.CreatedDate,
                Deadline = t.Deadline,
                IsOverdue = t.Deadline < DateTime.Now && !t.IsCompleted,
                IsCloseToDeadline = t.Deadline < DateTime.Now.AddDays(3) && !t.IsCompleted
            })
            .ToList();
    }
}