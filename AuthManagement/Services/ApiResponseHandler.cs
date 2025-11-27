using System.Net.Http.Json;
using AuthManagement.Models;

namespace AuthManagement.Services;

/// <summary>
/// Utility class for handling API responses and preventing JSON deserialization errors
/// </summary>
public static class ApiResponseHandler
{
    /// <summary>
    /// Handles API responses by checking content type and properly deserializing JSON or returning error messages
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    /// <param name="defaultErrorMessage">Default error message if response doesn't contain specific error details</param>
    /// <returns>Error message string</returns>
    public static async Task<string> GetErrorMessageAsync(HttpResponseMessage response, string defaultErrorMessage = "Operation failed")
    {
        try
        {
            var contentType = response.Content.Headers.ContentType?.MediaType;
            
            // Check if the response is JSON
            if (contentType?.Contains("application/json") == true)
            {
                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    return errorResponse?.Message ?? $"{defaultErrorMessage}. Status: {response.StatusCode}";
                }
                catch
                {
                    // If JSON deserialization fails, try reading as string
                    var content = await response.Content.ReadAsStringAsync();
                    return string.IsNullOrWhiteSpace(content) 
                        ? $"{defaultErrorMessage}. Status: {response.StatusCode}" 
                        : content;
                }
            }
            else
            {
                // Not JSON, read as string
                var content = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(content) 
                    ? $"{defaultErrorMessage}. Status: {response.StatusCode}" 
                    : content;
            }
        }
        catch
        {
            return $"{defaultErrorMessage}. Status: {response.StatusCode}";
        }
    }

    /// <summary>
    /// Safely reads JSON response from API, handling cases where content might not be JSON
    /// </summary>
    public static async Task<T?> ReadJsonResponseAsync<T>(HttpResponseMessage response) where T : class
    {
        try
        {
            var contentType = response.Content.Headers.ContentType?.MediaType;
            
            if (contentType?.Contains("application/json") == true)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }
}
