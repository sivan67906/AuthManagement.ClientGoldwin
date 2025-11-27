namespace AuthManagement.Models;

// Display model for RolePermissionMapping with resolved names
public class DisplayRolePermissionMapping
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Display model for PagePermissionMapping with resolved names
public class DisplayPagePermissionMapping
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Display model for PageFeatureMapping with resolved names
public class DisplayPageFeatureMapping
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string PageName { get; set; } = string.Empty;
    public Guid FeatureId { get; set; }
    public string FeatureName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
