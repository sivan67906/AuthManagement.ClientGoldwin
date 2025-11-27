namespace AuthManagement.Models;

public record RoleHierarchyMappingDto
{
    public Guid Id { get; init; }
    public Guid ParentRoleId { get; init; }
    public string ParentRoleName { get; init; } = string.Empty;
    public Guid? ParentDepartmentId { get; init; }
    public string? ParentDepartmentName { get; init; }
    public Guid ChildRoleId { get; init; }
    public string ChildRoleName { get; init; } = string.Empty;
    public Guid? ChildDepartmentId { get; init; }
    public string? ChildDepartmentName { get; init; }
    public int Level { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateRoleHierarchyMappingRequest
{
    public Guid ParentRoleId { get; set; }
    public Guid ChildRoleId { get; set; }
    public int Level { get; set; }
}

public record UpdateRoleHierarchyMappingRequest
{
    public Guid Id { get; set; }
    public Guid ParentRoleId { get; set; }
    public Guid ChildRoleId { get; set; }
    public int Level { get; set; }
}