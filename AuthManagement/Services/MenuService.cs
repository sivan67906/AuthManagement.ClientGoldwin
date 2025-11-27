using System.Net.Http.Json;
using System.Text.Json;
using AuthManagement.Models;

namespace AuthManagement.Services;

public class MenuService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MenuService> _logger;
    private List<MenuItemModel>? _cachedMenus;
    private DateTime? _cacheTimestamp;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    private readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
    private Task<List<MenuItemModel>>? _ongoingRequest;

    public MenuService(HttpClient httpClient, ILogger<MenuService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<MenuItemModel>> GetUserMenusAsync(bool forceRefresh = false)
    {
        // Check cache validity
        if (!forceRefresh && _cachedMenus != null && _cacheTimestamp.HasValue)
        {
            if (DateTime.UtcNow - _cacheTimestamp.Value < _cacheExpiration)
            {
                _logger.LogDebug("Returning cached menus");
                return _cachedMenus;
            }
        }

        // If a request is already in progress, wait for it
        if (_ongoingRequest != null)
        {
            _logger.LogDebug("Menu request already in progress, waiting for completion");
            return await _ongoingRequest;
        }

        await _cacheSemaphore.WaitAsync();
        try
        {
            // Double-check cache after acquiring semaphore
            if (!forceRefresh && _cachedMenus != null && _cacheTimestamp.HasValue)
            {
                if (DateTime.UtcNow - _cacheTimestamp.Value < _cacheExpiration)
                {
                    _logger.LogDebug("Returning cached menus (double-checked)");
                    return _cachedMenus;
                }
            }

            // Check again if another thread started a request
            if (_ongoingRequest != null)
            {
                _logger.LogDebug("Another thread started menu request, waiting for completion");
                return await _ongoingRequest;
            }

            _ongoingRequest = FetchMenusFromApiAsync();
            return await _ongoingRequest;
        }
        finally
        {
            _cacheSemaphore.Release();
        }
    }

    private async Task<List<MenuItemModel>> FetchMenusFromApiAsync()
    {
        try
        {
            _logger.LogInformation("Fetching user menus from API");
            var startTime = DateTime.UtcNow;

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemModel>>>("menu/user-menus");
            
            if (response?.Success == true && response.Data != null)
            {
                _cachedMenus = response.Data;
                _cacheTimestamp = DateTime.UtcNow;
                
                var fetchTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _logger.LogInformation("Successfully fetched {Count} menus in {FetchTime}ms", 
                    response.Data.Count, fetchTime);
                
                return _cachedMenus;
            }

            _logger.LogWarning("Failed to retrieve user menus - API returned unsuccessful response");
            return new List<MenuItemModel>();
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP error retrieving user menus: {Message}", httpEx.Message);
            return new List<MenuItemModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user menus: {Message}", ex.Message);
            return new List<MenuItemModel>();
        }
        finally
        {
            _ongoingRequest = null;
        }
    }

    public async Task<bool> CheckPageAccessAsync(string pageName)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<bool>>($"menu/check-page-access/{Uri.EscapeDataString(pageName)}");
            return response?.Success == true && response.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking page access for: {PageName}", pageName);
            return false;
        }
    }

    public async Task<bool> CheckPermissionAsync(string permissionName)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<bool>>($"menu/check-permission/{Uri.EscapeDataString(permissionName)}");
            return response?.Success == true && response.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permission: {PermissionName}", permissionName);
            return false;
        }
    }

    public async Task<List<string>> GetUserRolesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<string>>>("menu/user-roles");
            return response?.Data ?? new List<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user roles");
            return new List<string>();
        }
    }

    public async Task<Guid?> GetUserDepartmentAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Guid?>>("menu/user-department");
            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user department");
            return null;
        }
    }

    public void ClearMenuCache()
    {
        _cachedMenus = null;
        _cacheTimestamp = null;
        _logger.LogInformation("Menu cache cleared");
    }

    public void Dispose()
    {
        _cacheSemaphore?.Dispose();
    }
}

public class MenuItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public int Level { get; set; }
    public List<MenuItemModel> SubMenus { get; set; } = new();
    public List<PageModel> Pages { get; set; } = new();
}

public class PageModel
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public string? ApiEndpoint { get; set; }
    public string? HttpMethod { get; set; }
}
