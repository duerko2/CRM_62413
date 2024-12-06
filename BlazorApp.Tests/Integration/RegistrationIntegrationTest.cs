using System.Security.Cryptography;
using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Services;
using Moq;

namespace BlazorApp.TestsIES.Integration;

[TestFixture]
public class RegistrationServiceTest {

    private Mock<IRegistrationRepository> _registrationRepositoryMock;
    private Mock<ICompanySettingsRepository> _companySettingsRepositoryMock;
    private RegistrationService _registrationService;
    private CompanySettingsService _companySettingsService;

    [SetUp]
    public void SetUp() {
        _registrationRepositoryMock = new Mock<IRegistrationRepository>();
        _registrationService = new RegistrationService(_registrationRepositoryMock.Object);
        _companySettingsRepositoryMock = new Mock<ICompanySettingsRepository>();
        _companySettingsService = new CompanySettingsService(_companySettingsRepositoryMock.Object);
    }

    [Test]
    public void Successful_Registration_Test()
    {
        // An email is added to the company as a new user
        NewUser newUser = new NewUser { Email = "testuser@test.dk", Role = "Manager"};
        
        _companySettingsRepositoryMock.Setup(r => r.UserExists(newUser.Email)).Returns(false);
        _companySettingsRepositoryMock.Setup(r => r.GetCompanyIdForUserId(1)).Returns(1);
        
        _companySettingsService.AddUserToCompany(newUser, 1);
        
        // Assert the user is added
        _companySettingsRepositoryMock.Verify(repo => repo.AddUser(It.Is<NewUser>(u => u.Email == newUser.Email && u.CompanyId == 1)), Times.Once);
        
        
        // The user registers an account with the same email
        RegistrationModel registrationModel = new RegistrationModel
        {
            Email = newUser.Email,
        };
        
        _registrationRepositoryMock.Setup(repo => repo.RegistrationEmailExists(registrationModel.Email)).Returns(false);
        
        var emailExists = _registrationService.ConfirmEmail(registrationModel);
        
        // Assert the email is not already registered
        Assert.IsFalse(emailExists);
        
        // The user enters the remaining information
        registrationModel.Password = "password";
        registrationModel.ConfirmPassword = "password";
        registrationModel.Name = "Test User";
        
        _registrationService.Register(registrationModel);

        var salt = registrationModel.Salt;
        
        // Assert the user is registered with salted and hashed password
        _registrationRepositoryMock.Verify(repo => 
            repo.AddUser(
                It.Is<RegistrationModel>(u => 
                    u.Email == registrationModel.Email && 
                    u.Name == registrationModel.Name && 
                    registrationModel.Password != "password")
                ), Times.Once);
        
        
        
        // Test that the saved password can be verified with the correct password
        var savedPassword = registrationModel.Password;
        var correctPassword = "password";
        
        var saltedPassword = correctPassword + salt;
        
        // Hash the salted password            
        var sha256 = SHA256.Create();
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
        var hashedPassword = sha256.ComputeHash(passwordBytes);
        var hashedPasswordString = Convert.ToBase64String(hashedPassword);
            
        // Assert the saved salted and hashed password can later be verified with the correct password from a login form.
        Assert.AreEqual(savedPassword, hashedPasswordString);
    }
}