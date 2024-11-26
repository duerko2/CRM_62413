using System.Security.Claims;

namespace RolesAndPermissions;

public class UserAccessManager : IUserAccessManager
{
    Roles _roles = new Roles();
    
    public bool HasPermission(ClaimsPrincipal user, string permission)
    {
        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (role == null)
        {
            return false;
        }

        var permissions = _roles.GetPermissionsForRole(role);
        return permissions.Contains(permission);
    }
}
