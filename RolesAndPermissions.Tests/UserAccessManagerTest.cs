using System.Security.Claims;
using NUnit.Framework;
using RolesAndPermissions;

namespace RolesAndPermissions.Tests;

[TestFixture]
[TestOf(typeof(UserAccessManager))]
public class UserAccessManagerTest
{
    private UserAccessManager _userAccessManager;

    [Test]
    public void ManagerHasPermissionForManagerDashboard()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Manager")
        }));
        _userAccessManager = new UserAccessManager();

        // Act
        var hasPermission = _userAccessManager.HasPermission(user, "ManagerDashboard");

        // Assert
        Assert.IsTrue(hasPermission);
    }
    [Test]
    public void UserDoesNotHavePermissionForManagerDashboard()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "User")
        }));
        _userAccessManager = new UserAccessManager();

        // Act
        var hasPermission = _userAccessManager.HasPermission(user, "ManagerDashboard");

        // Assert
        Assert.IsFalse(hasPermission);
    }
    [Test]
    public void EmptyUserDoesNotHaveAnyPermissions()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        _userAccessManager = new UserAccessManager();

        // Act
        var hasPermission = _userAccessManager.HasPermission(user, "ManagerDashboard") || 
                            _userAccessManager.HasPermission(user, "UserDashboard");

        // Assert
        Assert.IsFalse(hasPermission);
    }
}