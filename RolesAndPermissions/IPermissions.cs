using System.Security.Claims;

namespace RolesAndPermissions;

public interface IPermissions
{ 
    bool HasPermission(ClaimsPrincipal user, string permission);
}