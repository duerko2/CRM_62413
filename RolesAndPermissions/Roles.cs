namespace RolesAndPermissions;

internal sealed class Roles
{
    private readonly Dictionary<string, List<string>> _rolePermissions = new()
    {
        { "Manager", ["ViewAllContacts", "EditCompany", "ManagerDashboard", "ViewAllPipelines"] },
        { "User", ["ViewOwnContacts", "UserDashboard", "ViewOwnPipelines"] },
    };
    
    public List<string> GetPermissionsForRole(string role)
    {
        if (_rolePermissions.TryGetValue(role, out var permissions))
        {
            return permissions;
        }
        return new List<string>(); // Return an empty list if the role isn't found
    }
}