using BlazorApp.Components.Pages;
using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services;

public class CompanySettingsService
{
    ICompanySettingsRepository _companySettingsRepository;
    public CompanySettingsService(ICompanySettingsRepository companySettingsRepository)
    {
        _companySettingsRepository = companySettingsRepository;
    }
    
    public void AddUserToCompany(NewUser newUser, int userContextUserId)
    {
        if(_companySettingsRepository.UserExists(newUser.Email))
        {
            throw new Exception("User already exists");
        }
        newUser.CompanyId = _companySettingsRepository.GetCompanyIdForUserId(userContextUserId);
        _companySettingsRepository.AddUser(newUser);
    }
}