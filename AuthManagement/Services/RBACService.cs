using System.Net.Http.Json;
using System.Net.Http.Headers;
using AuthManagement.Models;

namespace AuthManagement.Services;

public class RBACService
{
    private readonly HttpClient _httpClient;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public RBACService(HttpClient httpClient, JwtAuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _authStateProvider.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Department Methods
    public async Task<ApiResponse<List<DepartmentDto>>> GetAllDepartmentsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartmentDto>>>("api/department") 
               ?? new ApiResponse<List<DepartmentDto>> { Success = false, Data = new List<DepartmentDto>() };
    }

    public async Task<ApiResponse<DepartmentDto>> GetDepartmentAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<DepartmentDto>>($"api/department/{id}") 
               ?? new ApiResponse<DepartmentDto> { Success = false };
    }

    public async Task<ApiResponse<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/department", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<DepartmentDto>>() 
               ?? new ApiResponse<DepartmentDto> { Success = false };
    }

    public async Task<ApiResponse<DepartmentDto>> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/department/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<DepartmentDto>>() 
               ?? new ApiResponse<DepartmentDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteDepartmentAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/department/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Role Methods
    public async Task<ApiResponse<List<RoleDto>>> GetAllRolesAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<RoleDto>>>("api/role") 
               ?? new ApiResponse<List<RoleDto>> { Success = false, Data = new List<RoleDto>() };
    }

    public async Task<ApiResponse<RoleDto>> GetRoleByIdAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<RoleDto>>($"api/role/{id}") 
               ?? new ApiResponse<RoleDto> { Success = false };
    }

    public async Task<ApiResponse<RoleDto>> CreateRoleAsync(CreateRoleRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/role", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleDto>>() 
               ?? new ApiResponse<RoleDto> { Success = false };
    }

    public async Task<ApiResponse<RoleDto>> UpdateRoleAsync(Guid id, UpdateRoleRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/role/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleDto>>() 
               ?? new ApiResponse<RoleDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteRoleAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/role/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Permission Methods
    public async Task<ApiResponse<List<PermissionDto>>> GetAllPermissionsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<PermissionDto>>>("api/permission") 
               ?? new ApiResponse<List<PermissionDto>> { Success = false, Data = new List<PermissionDto>() };
    }

    public async Task<ApiResponse<PermissionDto>> GetPermissionAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<PermissionDto>>($"api/permission/{id}") 
               ?? new ApiResponse<PermissionDto> { Success = false };
    }

    public async Task<ApiResponse<PermissionDto>> CreatePermissionAsync(CreatePermissionRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/permission", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PermissionDto>>() 
               ?? new ApiResponse<PermissionDto> { Success = false };
    }

    public async Task<ApiResponse<PermissionDto>> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/permission/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PermissionDto>>() 
               ?? new ApiResponse<PermissionDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeletePermissionAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/permission/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Feature Methods
    public async Task<ApiResponse<List<FeatureDto>>> GetAllFeaturesAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<FeatureDto>>>("api/feature") 
               ?? new ApiResponse<List<FeatureDto>> { Success = false, Data = new List<FeatureDto>() };
    }

    public async Task<ApiResponse<FeatureDto>> GetFeatureAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<FeatureDto>>($"api/feature/{id}") 
               ?? new ApiResponse<FeatureDto> { Success = false };
    }

    public async Task<ApiResponse<FeatureDto>> CreateFeatureAsync(CreateFeatureRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/feature", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<FeatureDto>>() 
               ?? new ApiResponse<FeatureDto> { Success = false };
    }

    public async Task<ApiResponse<FeatureDto>> UpdateFeatureAsync(Guid id, UpdateFeatureRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/feature/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<FeatureDto>>() 
               ?? new ApiResponse<FeatureDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteFeatureAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/feature/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Page Methods
    public async Task<ApiResponse<List<PageDto>>> GetAllPagesAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<PageDto>>>("api/page") 
               ?? new ApiResponse<List<PageDto>> { Success = false, Data = new List<PageDto>() };
    }

    public async Task<ApiResponse<PageDto>> GetPageAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<PageDto>>($"api/page/{id}") 
               ?? new ApiResponse<PageDto> { Success = false };
    }

    public async Task<ApiResponse<PageDto>> CreatePageAsync(CreatePageRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/page", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PageDto>>() 
               ?? new ApiResponse<PageDto> { Success = false };
    }

    public async Task<ApiResponse<PageDto>> UpdatePageAsync(Guid id, UpdatePageRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/page/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PageDto>>() 
               ?? new ApiResponse<PageDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeletePageAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/page/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // RolePermissionMapping Methods
    public async Task<ApiResponse<List<RolePermissionMappingDto>>> GetAllRolePermissionMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<RolePermissionMappingDto>>>("api/rolepermissionmapping") 
               ?? new ApiResponse<List<RolePermissionMappingDto>> { Success = false, Data = new List<RolePermissionMappingDto>() };
    }

    public async Task<ApiResponse<RolePermissionMappingDto>> CreateRolePermissionMappingAsync(CreateRolePermissionMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/rolepermissionmapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RolePermissionMappingDto>>() 
               ?? new ApiResponse<RolePermissionMappingDto> { Success = false };
    }

    public async Task<ApiResponse<RolePermissionMappingDto>> UpdateRolePermissionMappingAsync(Guid id, UpdateRolePermissionMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/rolepermissionmapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RolePermissionMappingDto>>() 
               ?? new ApiResponse<RolePermissionMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteRolePermissionMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/rolepermissionmapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // PagePermissionMapping Methods
    public async Task<ApiResponse<List<PagePermissionMappingDto>>> GetAllPagePermissionMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<PagePermissionMappingDto>>>("api/pagepermissionmapping") 
               ?? new ApiResponse<List<PagePermissionMappingDto>> { Success = false, Data = new List<PagePermissionMappingDto>() };
    }

    public async Task<ApiResponse<PagePermissionMappingDto>> CreatePagePermissionMappingAsync(CreatePagePermissionMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/pagepermissionmapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PagePermissionMappingDto>>() 
               ?? new ApiResponse<PagePermissionMappingDto> { Success = false };
    }

    public async Task<ApiResponse<PagePermissionMappingDto>> UpdatePagePermissionMappingAsync(Guid id, UpdatePagePermissionMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/pagepermissionmapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PagePermissionMappingDto>>() 
               ?? new ApiResponse<PagePermissionMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeletePagePermissionMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/pagepermissionmapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // PageFeatureMapping Methods
    public async Task<ApiResponse<List<PageFeatureMappingDto>>> GetAllPageFeatureMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<PageFeatureMappingDto>>>("api/pagefeaturemapping") 
               ?? new ApiResponse<List<PageFeatureMappingDto>> { Success = false, Data = new List<PageFeatureMappingDto>() };
    }

    public async Task<ApiResponse<PageFeatureMappingDto>> CreatePageFeatureMappingAsync(CreatePageFeatureMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/pagefeaturemapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PageFeatureMappingDto>>() 
               ?? new ApiResponse<PageFeatureMappingDto> { Success = false };
    }

    public async Task<ApiResponse<PageFeatureMappingDto>> UpdatePageFeatureMappingAsync(Guid id, UpdatePageFeatureMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/pagefeaturemapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<PageFeatureMappingDto>>() 
               ?? new ApiResponse<PageFeatureMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeletePageFeatureMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/pagefeaturemapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // User Access Methods
    public async Task<ApiResponse<bool>> AssignRoleToUserAsync(AssignRoleRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/useraccess/assign-role", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<UserDto>>>("api/useraccess/users") 
               ?? new ApiResponse<List<UserDto>> { Success = false, Data = new List<UserDto>() };
    }

    public async Task<ApiResponse<UserAccessDto>> GetUserAccessAsync(Guid userId)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<UserAccessDto>>($"api/useraccess/{userId}") 
               ?? new ApiResponse<UserAccessDto> { Success = false };
    }

    // Get user's accessible pages for dynamic menu loading
    public async Task<ApiResponse<List<UserPageDto>>> GetUserPagesAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<UserPageDto>>>("api/useraccess/pages") 
               ?? new ApiResponse<List<UserPageDto>> { Success = false, Data = new List<UserPageDto>() };
    }

    // Get user's hierarchical navigation (Menu -> SubMenu -> Pages)
    public async Task<ApiResponse<UserNavigationDto>> GetUserNavigationAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<UserNavigationDto>>("api/useraccess/navigation") 
               ?? new ApiResponse<UserNavigationDto> { Success = false, Data = new UserNavigationDto() };
    }

    // Get user access by email instead of UserId
    public async Task<ApiResponse<UserAccessDto>> GetUserAccessByEmailAsync(string email)
    {
        await SetAuthorizationHeaderAsync();
        
        try
        {
            var encodedEmail = Uri.EscapeDataString(email);
            var response = await _httpClient.GetAsync($"api/useraccess/by-email/{encodedEmail}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new ApiResponse<UserAccessDto> 
                    { 
                        Success = false, 
                        Message = "Empty response from server" 
                    };
                }
                
                return await response.Content.ReadFromJsonAsync<ApiResponse<UserAccessDto>>() 
                       ?? new ApiResponse<UserAccessDto> { Success = false, Message = "Failed to deserialize response" };
            }
            else
            {
                return new ApiResponse<UserAccessDto> 
                { 
                    Success = false, 
                    Message = $"Server returned status code: {response.StatusCode}" 
                };
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserAccessDto> 
            { 
                Success = false, 
                Message = $"Error: {ex.Message}" 
            };
        }
    }

    // Role Hierarchy Mapping Methods
    public async Task<ApiResponse<List<RoleHierarchyMappingDto>>> GetAllRoleHierarchyMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<RoleHierarchyMappingDto>>>("auth/rolehierarchymapping") 
               ?? new ApiResponse<List<RoleHierarchyMappingDto>> { Success = false, Data = new List<RoleHierarchyMappingDto>() };
    }

    public async Task<ApiResponse<RoleHierarchyMappingDto>> CreateRoleHierarchyMappingAsync(CreateRoleHierarchyMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("auth/rolehierarchymapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleHierarchyMappingDto>>() 
               ?? new ApiResponse<RoleHierarchyMappingDto> { Success = false };
    }

    public async Task<ApiResponse<RoleHierarchyMappingDto>> UpdateRoleHierarchyMappingAsync(Guid id, UpdateRoleHierarchyMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"auth/rolehierarchymapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleHierarchyMappingDto>>() 
               ?? new ApiResponse<RoleHierarchyMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteRoleHierarchyMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"auth/rolehierarchymapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // User Role Mapping Methods
    public async Task<ApiResponse<List<UserRoleMappingDto>>> GetAllUserRoleMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<UserRoleMappingDto>>>("auth/userrolemapping") 
               ?? new ApiResponse<List<UserRoleMappingDto>> { Success = false, Data = new List<UserRoleMappingDto>() };
    }

    public async Task<ApiResponse<UserRoleMappingDto>> CreateUserRoleMappingAsync(CreateUserRoleMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("auth/userrolemapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<UserRoleMappingDto>>() 
               ?? new ApiResponse<UserRoleMappingDto> { Success = false };
    }

    public async Task<ApiResponse<UserRoleMappingDto>> UpdateUserRoleMappingAsync(Guid id, UpdateUserRoleMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"auth/userrolemapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<UserRoleMappingDto>>() 
               ?? new ApiResponse<UserRoleMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteUserRoleMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"auth/userrolemapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Role Department Mapping Methods
    public async Task<ApiResponse<List<RoleDepartmentMappingDto>>> GetAllRoleDepartmentMappingsAsync()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<RoleDepartmentMappingDto>>>("auth/roledepartmentmapping") 
               ?? new ApiResponse<List<RoleDepartmentMappingDto>> { Success = false, Data = new List<RoleDepartmentMappingDto>() };
    }

    public async Task<ApiResponse<RoleDepartmentMappingDto>> CreateRoleDepartmentMappingAsync(CreateRoleDepartmentMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("auth/roledepartmentmapping", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleDepartmentMappingDto>>() 
               ?? new ApiResponse<RoleDepartmentMappingDto> { Success = false };
    }

    public async Task<ApiResponse<RoleDepartmentMappingDto>> UpdateRoleDepartmentMappingAsync(Guid id, UpdateRoleDepartmentMappingRequest request)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"auth/roledepartmentmapping/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<RoleDepartmentMappingDto>>() 
               ?? new ApiResponse<RoleDepartmentMappingDto> { Success = false };
    }

    public async Task<ApiResponse<bool>> DeleteRoleDepartmentMappingAsync(Guid id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"auth/roledepartmentmapping/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>() 
               ?? new ApiResponse<bool> { Success = false };
    }

    // Get Roles By Department (helper method for Department-Role hierarchy)
    public async Task<ApiResponse<List<RoleDto>>> GetRolesByDepartmentAsync(Guid departmentId)
    {
        await SetAuthorizationHeaderAsync();
        var allRoles = await GetAllRolesAsync();
        if (allRoles.Success && allRoles.Data != null)
        {
            var filteredRoles = allRoles.Data.Where(r => r.DepartmentId == departmentId).ToList();
            return new ApiResponse<List<RoleDto>> { Success = true, Data = filteredRoles };
        }
        return new ApiResponse<List<RoleDto>> { Success = false, Data = new List<RoleDto>() };
    }
}
