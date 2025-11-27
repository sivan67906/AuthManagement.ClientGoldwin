using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuthManagement.Services;

/// <summary>
/// HTTP message handler that automatically attaches JWT bearer token to outgoing requests
/// Optimized for performance with cached token access
/// </summary>
public class AuthenticationMessageHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILogger<AuthenticationMessageHandler> _logger;
    private string? _cachedToken;
    private DateTime _tokenCacheTime = DateTime.MinValue;
    private readonly TimeSpan _tokenCacheDuration = TimeSpan.FromSeconds(30);

    public AuthenticationMessageHandler(
        AuthenticationStateProvider authenticationStateProvider,
        ILogger<AuthenticationMessageHandler> logger)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get token with caching to avoid repeated provider calls
            var token = await GetTokenWithCacheAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in authentication message handler");
            throw;
        }
    }

    private async Task<string?> GetTokenWithCacheAsync()
    {
        // Use cached token if still valid
        if (_cachedToken != null && 
            DateTime.UtcNow - _tokenCacheTime < _tokenCacheDuration)
        {
            return _cachedToken;
        }

        // Fetch fresh token
        if (_authenticationStateProvider is JwtAuthenticationStateProvider jwtProvider)
        {
            var token = await jwtProvider.GetTokenAsync();
            
            if (!string.IsNullOrWhiteSpace(token))
            {
                _cachedToken = token;
                _tokenCacheTime = DateTime.UtcNow;
            }
            
            return token;
        }

        return null;
    }

    /// <summary>
    /// Clears the cached token, forcing a fresh fetch on next request
    /// </summary>
    public void ClearTokenCache()
    {
        _cachedToken = null;
        _tokenCacheTime = DateTime.MinValue;
    }
}
