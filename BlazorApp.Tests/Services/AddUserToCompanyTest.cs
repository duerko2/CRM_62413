using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Services;
using Moq;

namespace BlazorApp.TestsIES.Services;

[TestFixture]
[TestOf(typeof(CompanySettingsService))]
public class AddUserToCompanyTest
{

    private Mock<ICompanySettingsRepository> _repositoryMock;
    private CompanySettingsService _companySettingsService;

    public AddUserToCompanyTest()
    {
        
    }
    [SetUp] // This method runs before each test
    public void SetUp()
    {
        _repositoryMock = new Mock<ICompanySettingsRepository>();
        _companySettingsService = new CompanySettingsService(_repositoryMock.Object);
    }
    
    [Test]
    public void AddUserToCompany_SuccessfulUserAddition_AddsUserToRepository()
    {
        // Arrange (like the ui)
        var newUser = new NewUser { Email = "testuser" };
        int userId = 1;
        int companyId = 42;

        _repositoryMock.Setup(r => r.UserExists(newUser.Email)).Returns(false);
        _repositoryMock.Setup(r => r.GetCompanyIdForUserId(userId)).Returns(companyId);

        // Act (Click add)
        _companySettingsService.AddUserToCompany(newUser, userId);

        // Assert (Check if user was added)
        Assert.AreEqual(companyId, newUser.CompanyId);
        _repositoryMock.Verify(r => r.AddUser(It.Is<NewUser>(u => u.Email == newUser.Email && u.CompanyId == companyId)), Times.Once);
    }
    [Test]
    public void AddUserToCompany_UserAlreadyExists_ThrowsException()
    {
        // Arrange
        var newUser = new NewUser { Email = "existingUser" };
        int userId = 1;

        _repositoryMock.Setup(r => r.UserExists(newUser.Email)).Returns(true);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => _companySettingsService.AddUserToCompany(newUser, userId));
        Assert.AreEqual("User already exists", exception.Message);

        // Never try to add user anyway
        _repositoryMock.Verify(r => r.AddUser(It.IsAny<NewUser>()), Times.Never);
    }
}