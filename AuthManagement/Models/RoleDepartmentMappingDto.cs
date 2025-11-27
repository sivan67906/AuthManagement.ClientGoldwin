namespace AuthManagement.Models;

public record RoleDepartmentMappingDto
{
    public Guid Id { get; init; }
    public Guid RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public bool IsPrimary { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateRoleDepartmentMappingRequest
{
    public Guid RoleId { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsPrimary { get; set; } = false;
}

public record UpdateRoleDepartmentMappingRequest
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsPrimary { get; set; }
}