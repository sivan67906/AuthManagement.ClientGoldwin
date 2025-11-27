using System.Net.Http.Json;
using AuthManagement.Models;

namespace AuthManagement.Services;

/// <summary>
/// Service responsible for fetching and managing hierarchical navigation menu structure
/// Integrates with the API to retrieve user-specific navigation based on roles and permissions
/// Optimized for performance with thread-safe caching and single-request guarantee
/// </summary>
public class NavigationService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NavigationService> _logger;
    private List<NavigationTreeNode> _navigationTree = new();
    private bool _isLoaded = false;
    private DateTime? _lastLoadTime;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);
    private bool _isLoading = false;
    private Task<bool>? _ongoingLoadTask;

    public event Action? OnNavigationChanged;

    public NavigationService(HttpClient httpClient, ILogger<NavigationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public List<NavigationTreeNode> NavigationTree => _navigationTree;
    public bool IsLoaded => _isLoaded && !IsCacheExpired();
    public bool IsLoading => _isLoading;

    private bool IsCacheExpired()
    {
        if (!_lastLoadTime.HasValue)
            return true;

        return DateTime.UtcNow - _lastLoadTime.Value > _cacheExpiration;
    }

    /// <summary>
    /// Fetches navigation menu from API and constructs hierarchical tree structure
    /// Thread-safe implementation ensures only one API call executes at a time
    /// Multiple concurrent calls will wait for the same request to complete
    /// </summary>
    public async Task<bool> LoadNavigationAsync()
    {
        // Return cached data if still valid
        if (_isLoaded && !IsCacheExpired())
        {
            _logger.LogDebug("Using cached navigation data");
            return true;
        }

        // If a load is already in progress, wait for it
        if (_isLoading && _ongoingLoadTask != null)
        {
            _logger.LogDebug("Navigation load already in progress, waiting for completion");
            return await _ongoingLoadTask;
        }

        await _loadSemaphore.WaitAsync();
        try
        {
            // Double-check after acquiring semaphore
            if (_isLoaded && !IsCacheExpired())
            {
                _logger.LogDebug("Using cached navigation data (double-checked)");
                return true;
            }

            if (_isLoading && _ongoingLoadTask != null)
            {
                _logger.LogDebug("Another thread started loading, waiting for completion");
                return await _ongoingLoadTask;
            }

            _isLoading = true;
            _ongoingLoadTask = LoadNavigationInternalAsync();

            return await _ongoingLoadTask;
        }
        finally
        {
            _loadSemaphore.Release();
        }
    }

    private async Task<bool> LoadNavigationInternalAsync()
    {
        try
        {
            _logger.LogInformation("Loading navigation from API...");
            var startTime = DateTime.UtcNow;

            // Use GetFromJsonAsync for better performance
            var apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemDto>>>("/menu/user-menus");

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                _logger.LogInformation("Successfully received {Count} menu items", apiResponse.Data.Count);

                _navigationTree = BuildNavigationTreeFromMenuItems(apiResponse.Data);
                _isLoaded = true;
                _lastLoadTime = DateTime.UtcNow;

                var loadTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                _logger.LogInformation("Navigation tree built in {LoadTime}ms with {NodeCount} nodes",
                    loadTime, CountNodes(_navigationTree));

                OnNavigationChanged?.Invoke();
                return true;
            }
            else
            {
                _logger.LogWarning("API returned unsuccessful response: {Message}", apiResponse?.Message);
                _isLoaded = false;
                return false;
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP error loading navigation: {Message}", httpEx.Message);
            _isLoaded = false;
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading navigation: {Message}", ex.Message);
            _isLoaded = false;
            return false;
        }
        finally
        {
            _isLoading = false;
            _ongoingLoadTask = null;
        }
    }

    /// <summary>
    /// Builds hierarchical navigation tree from menu items
    /// Optimized for performance with efficient processing
    /// </summary>
    private List<NavigationTreeNode> BuildNavigationTreeFromMenuItems(List<MenuItemDto> menuItems)
    {
        var tree = new List<NavigationTreeNode>();

        if (menuItems == null || !menuItems.Any())
            return tree;

        var orderedMenuItems = menuItems.OrderBy(m => m.DisplayOrder).ToList();

        foreach (var menuItem in orderedMenuItems)
        {
            var mainNode = new NavigationTreeNode
            {
                Id = menuItem.Id,
                Title = menuItem.Name,
                Icon = menuItem.Icon ?? "",
                Level = 0,
                IsExpanded = true,
                Children = new List<NavigationTreeNode>()
            };

            // Process pages directly under main menu
            if (menuItem.Pages?.Any() == true)
            {
                foreach (var page in menuItem.Pages.OrderBy(p => p.DisplayOrder))
                {
                    var pageNode = new NavigationTreeNode
                    {
                        Id = page.PageId,
                        Title = page.Name,
                        Url = page.Url,
                        Icon = "",
                        Level = 1,
                        IsExpanded = false,
                        Children = new List<NavigationTreeNode>()
                    };

                    mainNode.Children.Add(pageNode);
                }
            }

            // Process submenus
            if (menuItem.SubMenus?.Any() == true)
            {
                foreach (var subMenu in menuItem.SubMenus.OrderBy(s => s.DisplayOrder))
                {
                    var subNode = new NavigationTreeNode
                    {
                        Id = subMenu.Id,
                        Title = subMenu.Name,
                        Icon = subMenu.Icon ?? "",
                        Level = 1,
                        IsExpanded = false,
                        Children = new List<NavigationTreeNode>()
                    };

                    // Process pages
                    if (subMenu.Pages?.Any() == true)
                    {
                        foreach (var page in subMenu.Pages.OrderBy(p => p.DisplayOrder))
                        {
                            var pageNode = new NavigationTreeNode
                            {
                                Id = page.PageId,
                                Title = page.Name,
                                Url = page.Url,
                                Icon = "",
                                Level = 2,
                                IsExpanded = false,
                                Children = new List<NavigationTreeNode>()
                            };

                            subNode.Children.Add(pageNode);
                        }
                    }

                    mainNode.Children.Add(subNode);
                }
            }

            tree.Add(mainNode);
        }

        return tree;
    }

    private int CountNodes(List<NavigationTreeNode> nodes)
    {
        var count = nodes.Count;
        foreach (var node in nodes)
        {
            if (node.Children?.Any() == true)
                count += CountNodes(node.Children);
        }
        return count;
    }

    /// <summary>
    /// Refreshes navigation menu from API
    /// Forces a new load regardless of cache state
    /// </summary>
    public async Task<bool> RefreshNavigationAsync()
    {
        await _loadSemaphore.WaitAsync();
        try
        {
            _isLoaded = false;
            _lastLoadTime = null;
            return await LoadNavigationAsync();
        }
        finally
        {
            _loadSemaphore.Release();
        }
    }

    /// <summary>
    /// Clears cached navigation data
    /// </summary>
    public void ClearNavigation()
    {
        _navigationTree.Clear();
        _isLoaded = false;
        _lastLoadTime = null;
        OnNavigationChanged?.Invoke();
        _logger.LogInformation("Navigation cache cleared");
    }

    public void Dispose()
    {
        _loadSemaphore?.Dispose();
    }
}
