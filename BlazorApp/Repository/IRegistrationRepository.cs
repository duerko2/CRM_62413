using BlazorApp.Models;

namespace BlazorApp.Repository;

public interface IRegistrationRepository
{
    public bool RegistrationEmailExists(string registrationModelEmail);

    public bool UserNameExists(string registrationModelUsername);

    public void AddUser(RegistrationModel registrationModel);
}