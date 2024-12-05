using System.Net.Mail;
using System.Security.Cryptography;
using BlazorApp.Models;
using BlazorApp.Repository;

namespace BlazorApp.Services;

public class RegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    public RegistrationService(IRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }
    
    public bool ConfirmEmail(RegistrationModel registrationModel)
    {
        try
        {
            MailAddress mailAddress = new(registrationModel.Email);
        }
        catch (FormatException)
        {
            throw new RegistrationException("Invalid email address");
        }

        return _registrationRepository.RegistrationEmailExists(registrationModel.Email);
    }

    public void Register(RegistrationModel registrationModel)
    {
        if(registrationModel.Password != registrationModel.ConfirmPassword)
        {
            throw new RegistrationException("Passwords do not match");
        }
        if(registrationModel.Password.Length < 8)
        {
            throw new RegistrationException("Password must be at least 8 characters long");
        }
        if(_registrationRepository.UserNameExists(registrationModel.Username))
        {
            throw new RegistrationException("Username already exists");
        }

        Byte[] userSalt = RandomNumberGenerator.GetBytes(16);
        string saltString = Convert.ToBase64String(userSalt);
        registrationModel.Salt = saltString;

        var saltedPassword = registrationModel.Password + saltString;
        
        using var sha256 = SHA256.Create();
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
        var hashedPassword = sha256.ComputeHash(passwordBytes);
        registrationModel.Password = Convert.ToBase64String(hashedPassword);
        
        _registrationRepository.AddUser(registrationModel);
    }
    
    public class RegistrationException: Exception
    {
        public RegistrationException(string message) : base(message)
        {
        }
    }
}