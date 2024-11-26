using System.Security.Claims;

namespace RolesAndPermissions;

public interface IUserAccessManager
{ 
    bool HasPermission(ClaimsPrincipal user, string permission);
}