using BlazorApp.Models;

namespace BlazorApp.Repository;

public interface ICompanySettingsRepository
{
    bool UserExists(string newUserUsername);
    void AddUser(NewUser newUser);
    int GetCompanyIdForUserId(int userContextUserId);
}