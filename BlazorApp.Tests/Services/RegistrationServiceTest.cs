using BlazorApp.Models;
using BlazorApp.Repository;
using BlazorApp.Services;
using Moq;

namespace BlazorApp.TestsIES.Services;

[TestFixture]
[TestOf(typeof(RegistrationService))]
public class RegistrationServiceTest {

    private Mock<IRegistrationRepository> _registrationRepositoryMock;
    private RegistrationService _registrationService;

    [SetUp]
    public void SetUp() {
        _registrationRepositoryMock = new Mock<IRegistrationRepository>();
        _registrationService = new RegistrationService(_registrationRepositoryMock.Object);
    }

    [Test]
    public void ValidEmail_EmailConfirmation_ThrowsNoException()
    {
        // Arrange
        var registrationModel = new RegistrationModel
        {
            Email = "validemail@validdomain.dk"
        };
        _registrationRepositoryMock.Setup(repo => repo.RegistrationEmailExists(registrationModel.Email))
            .Returns(true);
        
        // Act
        var result = _registrationService.ConfirmEmail(registrationModel);
        
        // Assert
        Assert.IsTrue(result);
    }
    [Test]
    public void InvalidEmailFormat_EmailConfirmation_ThrowsException()
    {
        // Arrange
        var registrationModel = new RegistrationModel
        {
            Email = "invalidemail"
        };
        _registrationRepositoryMock.Setup(repo => repo.RegistrationEmailExists(registrationModel.Email))
            .Returns(false);
        
        // Act
        var exception = Assert.Throws<RegistrationService.RegistrationException>(() => _registrationService.ConfirmEmail(registrationModel));
        
        
        // Assert
        Assert.AreEqual("Invalid email address", exception.Message);
    }

    [Test]
    public void ValidRegistration_Registration_ThrowsNoException()
    {
        // Arrange
        var registrationModel = new RegistrationModel
        {
            Email = "test@test.dk",
            Username = "test",
            Name = "Test Test",
            Password = "password",
            ConfirmPassword = "password"
        };
        _registrationRepositoryMock.Setup(repo => repo.UserNameExists(registrationModel.Username))
            .Returns(false);

        // Act
        _registrationService.Register(registrationModel);

        // Assert
        _registrationRepositoryMock.Verify(repo => repo.AddUser(registrationModel), Times.Once);
    }

    [Test]
    public void PasswordsDoNotMatch_Registration_ThrowsException()
    {
        // Arrange
        var registrationModel = new RegistrationModel
        {
            Email = "test@test.dk",
            Username = "test",
            Name = "Test Test",
            Password = "password",
            ConfirmPassword = "wrong"
        };

        // Act & Assert
        var exception =
            Assert.Throws<RegistrationService.RegistrationException>(() =>
                _registrationService.Register(registrationModel));
        Assert.AreEqual("Passwords do not match", exception.Message);
    }

}