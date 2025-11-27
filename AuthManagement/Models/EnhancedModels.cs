namespace AuthManagement.Models;

// Enhanced Feature Models with Hierarchy Support
public class EnhancedFeatureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public string? ParentFeatureName { get; set; }
    public string? RouteUrl { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<EnhancedFeatureDto> ChildFeatures { get; set; } = new();
}

public class CreateEnhancedFeatureRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public string? RouteUrl { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; } = 0;
}

public class UpdateEnhancedFeatureRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsMainMenu { get; set; }
    public Guid? ParentFeatureId { get; set; }
    public string? RouteUrl { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
}

// Enhanced Page Models
public class EnhancedPageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Context { get; set; }
    public string? ApiEndpoint { get; set; }
    public string? HttpMethod { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<string> AssignedPermissions { get; set; } = new();
    public List<string> AssignedFeatures { get; set; } = new();
}

public class CreateEnhancedPageRequest
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Context { get; set; }
    public string? ApiEndpoint { get; set; }
    public string? HttpMethod { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateEnhancedPageRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Context { get; set; }
    public string? ApiEndpoint { get; set; }
    public string? HttpMethod { get; set; }
    public int DisplayOrder { get; set; }
}

// Enhanced Mapping Models with Details
public class EnhancedRolePermissionMappingDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EnhancedPagePermissionMappingDto
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EnhancedPageFeatureMappingDto
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public Guid FeatureId { get; set; }
    public string FeatureName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EnhancedUserRoleMappingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EnhancedRoleDepartmentMappingDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Role Hierarchy Models
public class EnhancedRoleHierarchyDto
{
    public Guid Id { get; set; }
    public Guid ParentRoleId { get; set; }
    public string ParentRoleName { get; set; } = string.Empty;
    public Guid ChildRoleId { get; set; }
    public string ChildRoleName { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateRoleHierarchyRequest
{
    public Guid ParentRoleId { get; set; }
    public Guid ChildRoleId { get; set; }
    public int Level { get; set; }
}

// Navigation Tree Models for MudBlazor TreeView
public class NavigationTreeNode
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public int Level { get; set; }
    public bool IsExpanded { get; set; }
    public List<NavigationTreeNode> Children { get; set; } = new();
}

// Search and Filter Models
public class SearchFilterRequest
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public Dictionary<string, string>? Filters { get; set; }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
