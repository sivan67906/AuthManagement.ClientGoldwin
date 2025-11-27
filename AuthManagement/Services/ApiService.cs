using System.Net.Http.Json;
using System.Text.Json;
using AuthManagement.Models;

namespace AuthManagement.Services;

/// <summary>
/// Comprehensive API service providing standardized CRUD operations for all entities
/// Includes error handling, loading states, and notification support
/// </summary>
public class ApiService<T> where T : class
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public bool IsLoading { get; private set; }
    public string? LastError { get; private set; }

    public ApiService(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<List<T>?> GetAllAsync()
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var response = await _httpClient.GetAsync($"/api/{_endpoint}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<T>>>();
                return apiResponse?.Data;
            }
            else
            {
                LastError = $"Error fetching data: {response.StatusCode}";
                return null;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return null;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var response = await _httpClient.GetAsync($"/api/{_endpoint}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                return apiResponse?.Data;
            }
            else
            {
                LastError = $"Error fetching item: {response.StatusCode}";
                return null;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return null;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<bool> CreateAsync<TRequest>(TRequest request) where TRequest : class
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/{_endpoint}", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                LastError = $"Error creating item: {response.StatusCode} - {errorContent}";
                return false;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<bool> UpdateAsync<TRequest>(Guid id, TRequest request) where TRequest : class
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/{_endpoint}/{id}", request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                LastError = $"Error updating item: {response.StatusCode} - {errorContent}";
                return false;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/{_endpoint}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                LastError = $"Error deleting item: {response.StatusCode} - {errorContent}";
                return false;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<PagedResult<T>?> GetPagedAsync(SearchFilterRequest request)
    {
        IsLoading = true;
        LastError = null;
        try
        {
            var queryString = BuildQueryString(request);
            var response = await _httpClient.GetAsync($"/api/{_endpoint}/paged?{queryString}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PagedResult<T>>();
            }
            else
            {
                LastError = $"Error fetching paged data: {response.StatusCode}";
                return null;
            }
        }
        catch (Exception ex)
        {
            LastError = $"Exception: {ex.Message}";
            return null;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private string BuildQueryString(SearchFilterRequest request)
    {
        var queryParams = new List<string>
        {
            $"pageNumber={request.PageNumber}",
            $"pageSize={request.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            queryParams.Add($"searchTerm={Uri.EscapeDataString(request.SearchTerm)}");

        if (!string.IsNullOrWhiteSpace(request.SortBy))
            queryParams.Add($"sortBy={request.SortBy}");

        queryParams.Add($"sortDescending={request.SortDescending}");

        if (request.Filters != null)
        {
            foreach (var filter in request.Filters)
            {
                queryParams.Add($"{filter.Key}={Uri.EscapeDataString(filter.Value)}");
            }
        }

        return string.Join("&", queryParams);
    }
}
