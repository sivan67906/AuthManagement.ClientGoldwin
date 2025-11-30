using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuthManagement.Services;

/// <summary>
/// HTTP message handler that automatically attaches JWT bearer token to outgoing requests.
/// Fetches token from JwtAuthenticationStateProvider which persists to localStorage.
/// </summary>
public class AuthenticationMessageHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILogger<AuthenticationMessageHandler> _logger;

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
            // Get token from the auth provider (which loads from localStorage if needed)
            if (_authenticationStateProvider is JwtAuthenticationStateProvider jwtProvider)
            {
                var token = await jwtProvider.GetTokenAsync();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    _logger.LogDebug("Added Bearer token to request: {Path}", request.RequestUri?.PathAndQuery);
                }
                else
                {
                    _logger.LogDebug("No token available for request: {Path}", request.RequestUri?.PathAndQuery);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Log 401 errors for debugging
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("401 Unauthorized for request: {Path}", request.RequestUri?.PathAndQuery);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in authentication message handler for {Path}",
                request.RequestUri?.PathAndQuery);
            throw;
        }
    }
}
