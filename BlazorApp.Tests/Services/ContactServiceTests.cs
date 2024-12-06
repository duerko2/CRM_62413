using NUnit.Framework;
using Moq;
using BlazorApp;
using BlazorApp.Models;
using BlazorApp.Persistence.Entities;
using BlazorApp.Repository;
using BlazorApp.Services;
using Contact = BlazorApp.Models.Contact;

namespace BlazorApp.TestsIES.Services;

[TestFixture]  // Marks the class as a test fixture in NUnit
public class ContactServiceTests
{
    private Mock<IContactRepository> _contactRepositoryMock;
    private Mock<IActivityLogRepository> _activityLogRepositoryMock;
    private ContactService _service;

    [SetUp]  // SetUp runs before each test method
    public void SetUp()
    {
        _contactRepositoryMock = new Mock<IContactRepository>();
        _activityLogRepositoryMock = new Mock<IActivityLogRepository>();

        // Instantiate the service with mocked dependencies
        _service = new ContactService(
            _contactRepositoryMock.Object,
            _activityLogRepositoryMock.Object
        );
    }

    [Test]  // Marks this method as a test method
    public void GetContacts_ShouldReturnContactsForUser()
    {
        // Arrange
        int userId = 1;
        var expectedContacts = new List<ContactListRow>
        {
            new ContactListRow { Id = 1, Name = "Contact A" },
            new ContactListRow { Id = 2, Name = "Contact B" }
        };

        _contactRepositoryMock.Setup(repo => repo.GetContactsForUser(userId))
            .Returns(expectedContacts);

        // Act
        var result = _service.GetContacts(userId);

        // Assert
        Assert.AreEqual(expectedContacts, result);
        _contactRepositoryMock.Verify(repo => repo.GetContactsForUser(userId), Times.Once);
    }

    [Test]
    public void GetContactById_ShouldReturnContactById()
    {
        // Arrange
        int contactId = 1;
        var expectedContact = new Contact { Id = contactId, Name = "John Doe" };

        _contactRepositoryMock.Setup(repo => repo.GetContact(contactId))
            .Returns(expectedContact);

        // Act
        var result = _service.GetContactById(contactId);

        // Assert
        Assert.AreEqual(expectedContact, result);
        _contactRepositoryMock.Verify(repo => repo.GetContact(contactId), Times.Once);
    }

    [Test]
    public void SaveContact_ShouldAddNewContactAndLogActivity()
    {
        // Arrange
        var newContact = new Contact { Name = "New Contact" };
        int userId = 1;

        // Act
        _service.SaveContact(newContact, userId);

        // Assert
        _contactRepositoryMock.Verify(repo => repo.AddContact(newContact), Times.Once);
        _activityLogRepositoryMock.Verify(repo => repo.AddActivityLog(It.Is<ActivityLog>(
            log => log.Type == (int)ActivityLogType.ContactCreated && log.ContactId == newContact.Id
        )), Times.Once);
    }

    [Test]
    public void SaveContact_ShouldUpdateExistingContactAndLogActivity()
    {
        // Arrange
        var existingContact = new Contact { Id = 1, Name = "Updated Contact" };
        int userId = 1;

        // Act
        _service.SaveContact(existingContact, userId);

        // Assert
        _contactRepositoryMock.Verify(repo => repo.UpdateContact(existingContact), Times.Once);
        _activityLogRepositoryMock.Verify(repo => repo.AddActivityLog(It.Is<ActivityLog>(
            log => log.Type == (int)ActivityLogType.ContactUpdated && log.ContactId == existingContact.Id
        )), Times.Once);
    }

    [Test]
    public void SaveContact_ShouldAssignUserIdIfNotSet()
    {
        // Arrange
        var newContact = new Contact { Name = "New Contact" };
        int userId = 1;

        // Act
        _service.SaveContact(newContact, userId);

        // Assert
        Assert.AreEqual(userId, newContact.UserId);
        _contactRepositoryMock.Verify(repo => repo.AddContact(newContact), Times.Once);
    }
}
