using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace AuthManagement.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private string? _accessToken;
    private bool _twoFactorVerified;
    
    private AuthenticationState? _cachedAuthState;
    private DateTime _cacheTimestamp = DateTime.MinValue;
    private readonly TimeSpan _cacheValidity = TimeSpan.FromSeconds(1);
    private readonly object _stateLock = new();

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        lock (_stateLock)
        {
            if (_cachedAuthState != null && 
                (DateTime.UtcNow - _cacheTimestamp) < _cacheValidity)
            {
                return Task.FromResult(_cachedAuthState);
            }

            ClaimsPrincipal currentUser;

            if (string.IsNullOrWhiteSpace(_accessToken) || !_twoFactorVerified)
            {
                currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }
            else
            {
                try
                {
                    var token = _tokenHandler.ReadJwtToken(_accessToken);
                    var identity = new ClaimsIdentity(token.Claims, "jwt");
                    currentUser = new ClaimsPrincipal(identity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AUTH] Token parsing failed: {ex.Message}");
                    currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }

            _cachedAuthState = new AuthenticationState(currentUser);
            _cacheTimestamp = DateTime.UtcNow;

            return Task.FromResult(_cachedAuthState);
        }
    }

    public Task SetTokenAsync(string? token)
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Setting token: {!string.IsNullOrWhiteSpace(token)}");
            _accessToken = token;
            _twoFactorVerified = false;
            _cachedAuthState = null;
        }
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        Console.WriteLine($"[AUTH] Token set and state notification sent");
        
        return Task.CompletedTask;
    }

    // In JwtAuthenticationStateProvider.cs - make notification truly synchronous
    public Task SetTwoFactorVerifiedAsync(bool verified)
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH {DateTime.UtcNow:HH:mm:ss.fff}] Setting 2FA verified: {verified}");
            _twoFactorVerified = verified;
            _cachedAuthState = null;
            _cacheTimestamp = DateTime.MinValue;

            // Immediately notify - no async delay
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            Console.WriteLine($"[AUTH {DateTime.UtcNow:HH:mm:ss.fff}] Notification sent");
        }

        return Task.CompletedTask;
    }

    public Task<bool> IsTwoFactorVerifiedAsync() 
    {
        lock (_stateLock)
        {
            return Task.FromResult(_twoFactorVerified);
        }
    }

    public Task ClearAsync()
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Clearing authentication state");
            _accessToken = null;
            _twoFactorVerified = false;
            _cachedAuthState = null;
        }
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        Console.WriteLine($"[AUTH] Authentication cleared");
        
        return Task.CompletedTask;
    }

    public Task<string?> GetTokenAsync()
    {
        lock (_stateLock)
        {
            return Task.FromResult(_accessToken);
        }
    }

    public void InvalidateCache()
    {
        lock (_stateLock)
        {
            _cachedAuthState = null;
        }
    }

    public Task SetAuthenticationAsync(string? token)
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Setting full authentication with token: {!string.IsNullOrWhiteSpace(token)}");
            _accessToken = token;
            _twoFactorVerified = true;
            _cachedAuthState = null;
        }
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        Console.WriteLine($"[AUTH] Full authentication set and state notification sent");
        
        return Task.CompletedTask;
    }
}