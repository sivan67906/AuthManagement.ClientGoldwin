namespace AuthManagement.Models;

public class UserWithoutRoleDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
}