using BlazorApp.Models;

namespace BlazorApp.Repository;

public interface IDashboardRepository
{
    List<TaskViewModel> GetTasksForUser(int userContextUserId);
    List<TaskViewModel> GetTasksForCompany(int userContextUserId);
}