using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using AuthManagement.Models;
using AuthManagement.Constants;

namespace AuthManagement.Services;

public class PermissionService
{
    private readonly RBACService _rbacService;
    private readonly AuthenticationStateProvider _authStateProvider;
    private UserAccessDto? _userAccess;
    private DateTime? _lastLoaded;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

    public PermissionService(RBACService rbacService, AuthenticationStateProvider authStateProvider)
    {
        _rbacService = rbacService;
        _authStateProvider = authStateProvider;
    }

    private async Task<UserAccessDto?> GetUserAccessAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _userAccess != null && _lastLoaded.HasValue && 
            DateTime.UtcNow - _lastLoaded.Value < _cacheExpiration)
        {
            return _userAccess;
        }

        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var email = user.FindFirst("email")?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        var response = await _rbacService.GetUserAccessByEmailAsync(email);
        if (response.Success && response.Data != null)
        {
            _userAccess = response.Data;
            _lastLoaded = DateTime.UtcNow;
            return _userAccess;
        }

        return null;
    }

    public async Task<bool> HasPermissionAsync(string pageName, string permissionName)
    {
        var userAccess = await GetUserAccessAsync();
        if (userAccess == null) return false;

        // SuperAdmin has all permissions
        if (userAccess.Roles.Contains(UIRoles.SuperAdmin))
        {
            return true;
        }

        // Check if user has the specific permission
        return userAccess.Permissions.Contains(permissionName);
    }

    public async Task<PagePermissions> GetPagePermissionsAsync(string pageName)
    {
        var userAccess = await GetUserAccessAsync();
        if (userAccess == null)
        {
            return new PagePermissions();
        }

        // SuperAdmin has all permissions
        if (userAccess.Roles.Contains(UIRoles.SuperAdmin))
        {
            return new PagePermissions
            {
                CanView = true,
                CanAdd = true,
                CanEdit = true,
                CanDelete = true
            };
        }

        // Check specific permissions based on page
        var pagePermissions = new PagePermissions();
        var permissionPrefix = pageName.ToLower();

        pagePermissions.CanView = userAccess.Permissions.Any(p => 
            p.Equals($"{permissionPrefix}.view", StringComparison.OrdinalIgnoreCase) ||
            p.Equals($"view{pageName}", StringComparison.OrdinalIgnoreCase) ||
            p.Equals("view", StringComparison.OrdinalIgnoreCase));

        pagePermissions.CanAdd = userAccess.Permissions.Any(p => 
            p.Equals($"{permissionPrefix}.add", StringComparison.OrdinalIgnoreCase) ||
            p.Equals($"add{pageName}", StringComparison.OrdinalIgnoreCase) ||
            p.Equals("create", StringComparison.OrdinalIgnoreCase));

        pagePermissions.CanEdit = userAccess.Permissions.Any(p => 
            p.Equals($"{permissionPrefix}.edit", StringComparison.OrdinalIgnoreCase) ||
            p.Equals($"edit{pageName}", StringComparison.OrdinalIgnoreCase) ||
            p.Equals("update", StringComparison.OrdinalIgnoreCase) ||
            p.Equals("edit", StringComparison.OrdinalIgnoreCase));

        pagePermissions.CanDelete = userAccess.Permissions.Any(p => 
            p.Equals($"{permissionPrefix}.delete", StringComparison.OrdinalIgnoreCase) ||
            p.Equals($"delete{pageName}", StringComparison.OrdinalIgnoreCase) ||
            p.Equals("delete", StringComparison.OrdinalIgnoreCase));

        // Role-based fallback permissions
        ApplyRoleBasedPermissions(userAccess.Roles, pagePermissions);

        return pagePermissions;
    }

    private void ApplyRoleBasedPermissions(List<string> roles, PagePermissions permissions)
    {
        // If user has Admin roles (FinanceAdmin, HRAdmin, etc.)
        if (roles.Any(r => r.Contains("Admin") && r != UIRoles.SuperAdmin))
        {
            permissions.CanView = true;
            permissions.CanAdd = true;
            permissions.CanEdit = true;
            permissions.CanDelete = true;
        }
        // Manager roles
        else if (roles.Any(r => r.Contains("Manager")))
        {
            permissions.CanView = true;
            permissions.CanAdd = true;
            permissions.CanEdit = true;
            permissions.CanDelete = false; // Managers cannot delete
        }
        // Analyst/Executive roles
        else if (roles.Any(r => r.Contains("Analyst") || r.Contains("Executive")))
        {
            permissions.CanView = true;
            permissions.CanAdd = true;
            permissions.CanEdit = false;
            permissions.CanDelete = false;
        }
        // Staff roles - view only
        else if (roles.Any(r => r.Contains("Staff")))
        {
            permissions.CanView = true;
            permissions.CanAdd = false;
            permissions.CanEdit = false;
            permissions.CanDelete = false;
        }
    }

    public async Task<bool> IsSuperAdminAsync()
    {
        var userAccess = await GetUserAccessAsync();
        return userAccess?.Roles.Contains(UIRoles.SuperAdmin) ?? false;
    }

    public async Task<List<string>> GetUserRolesAsync()
    {
        var userAccess = await GetUserAccessAsync();
        return userAccess?.Roles ?? new List<string>();
    }

    public async Task<string?> GetUserDepartmentAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.FindFirst("Department")?.Value;
    }

    public void ClearCache()
    {
        _userAccess = null;
        _lastLoaded = null;
    }
}

public class PagePermissions
{
    public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }

    public bool HasAnyPermission => CanView || CanAdd || CanEdit || CanDelete;
}
