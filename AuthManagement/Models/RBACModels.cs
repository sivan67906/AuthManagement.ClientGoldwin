namespace AuthManagement.Models;

// Department Models
public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateDepartmentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// Role Models
public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? DepartmentId { get; set; }
}

// Permission Models
public class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePermissionRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdatePermissionRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// Feature Models
public class FeatureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RouteUrl { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public string? ParentFeatureName { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateFeatureRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RouteUrl { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
    public string? Icon { get; set; }
}

public class UpdateFeatureRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RouteUrl { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
    public string? Icon { get; set; }
}

// Page Models
public class PageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePageRequest
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdatePageRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

// Mapping Models
public class RolePermissionMappingDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateRolePermissionMappingRequest
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}

public class UpdateRolePermissionMappingRequest
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}

public class PagePermissionMappingDto
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePagePermissionMappingRequest
{
    public Guid PageId { get; set; }
    public Guid PermissionId { get; set; }
}

public class UpdatePagePermissionMappingRequest
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid PermissionId { get; set; }
}

public class PageFeatureMappingDto
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid FeatureId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePageFeatureMappingRequest
{
    public Guid PageId { get; set; }
    public Guid FeatureId { get; set; }
}

public class UpdatePageFeatureMappingRequest
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public Guid FeatureId { get; set; }
}

// User Access Models
public class AssignRoleRequest
{
    public string EmailId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}

public class UserAccessDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public List<PageAccessDto> Pages { get; set; } = new();
}

public class PageAccessDto
{
    public Guid PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
    public List<string> Features { get; set; } = new();
}

// User Page DTO for dynamic menu loading
public class UserPageDto
{
    public Guid PageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public List<string> RequiredPermissions { get; set; } = new();
    public List<string> Features { get; set; } = new();
}

// Hierarchical Navigation Models (Menu -> SubMenu -> Pages)
public class MenuItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsMainMenu { get; set; }
    public int Level { get; set; }
    public List<SubMenuItemDto> SubMenus { get; set; } = new();
    public List<PageItemDto> Pages { get; set; } = new();
}

public class SubMenuItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public List<PageItemDto> Pages { get; set; } = new();
}

public class PageItemDto
{
    public Guid PageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public List<string> RequiredPermissions { get; set; } = new();
}

public class UserNavigationDto
{
    public List<MenuItemDto> MenuItems { get; set; } = new();
    public string UserEmail { get; set; } = string.Empty;
    public List<string> UserRoles { get; set; } = new();
    public string? DepartmentName { get; set; }
}

// Update Role Request
public class UpdateRoleRequest
{
    public Guid RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? DepartmentId { get; set; }
}
