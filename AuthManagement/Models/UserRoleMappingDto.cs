namespace AuthManagement.Models;

public record UserRoleMappingDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserEmail { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public Guid RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public Guid? DepartmentId { get; init; }
    public string? DepartmentName { get; init; }
    public string AssignedByEmail { get; init; } = string.Empty;
    public DateTime AssignedAt { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateUserRoleMappingRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string AssignedByEmail { get; set; } = string.Empty;
}

public record UpdateUserRoleMappingRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string AssignedByEmail { get; set; } = string.Empty;
}