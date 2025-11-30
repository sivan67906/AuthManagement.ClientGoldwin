using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace AuthManagement.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly IJSRuntime _jsRuntime;
    private string? _accessToken;
    private bool _isInitialized;

    private AuthenticationState? _cachedAuthState;
    private DateTime _cacheTimestamp = DateTime.MinValue;
    private readonly TimeSpan _cacheValidity = TimeSpan.FromSeconds(1);
    private readonly object _stateLock = new();

    // Pending 2FA session storage for authenticator/email verification flow
    private string? _pendingTwoFactorEmail;
    private string? _pendingTwoFactorToken;
    private string? _pendingTwoFactorType;

    // Storage keys
    private const string TokenKey = "auth_token";
    private const string Pending2FAEmailKey = "pending_2fa_email";
    private const string Pending2FATokenKey = "pending_2fa_token";
    private const string Pending2FATypeKey = "pending_2fa_type";

    public JwtAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Initialize from storage on first call
        if (!_isInitialized)
        {
            await InitializeFromStorageAsync();
        }

        lock (_stateLock)
        {
            if (_cachedAuthState != null &&
                (DateTime.UtcNow - _cacheTimestamp) < _cacheValidity)
            {
                return _cachedAuthState;
            }

            ClaimsPrincipal currentUser;

            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }
            else
            {
                try
                {
                    var token = _tokenHandler.ReadJwtToken(_accessToken);

                    // Check if token is expired
                    if (token.ValidTo < DateTime.UtcNow)
                    {
                        Console.WriteLine($"[AUTH] Token expired at {token.ValidTo}");
                        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                        _accessToken = null;
                    }
                    else
                    {
                        var identity = new ClaimsIdentity(token.Claims, "jwt");
                        currentUser = new ClaimsPrincipal(identity);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AUTH] Token parsing failed: {ex.Message}");
                    currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }

            _cachedAuthState = new AuthenticationState(currentUser);
            _cacheTimestamp = DateTime.UtcNow;

            return _cachedAuthState;
        }
    }

    private async Task InitializeFromStorageAsync()
    {
        try
        {
            // Load token from localStorage
            _accessToken = await GetFromStorageAsync(TokenKey);

            // Load pending 2FA session
            _pendingTwoFactorEmail = await GetFromStorageAsync(Pending2FAEmailKey);
            _pendingTwoFactorToken = await GetFromStorageAsync(Pending2FATokenKey);
            _pendingTwoFactorType = await GetFromStorageAsync(Pending2FATypeKey);

            Console.WriteLine($"[AUTH] Initialized from storage. Token: {!string.IsNullOrWhiteSpace(_accessToken)}");

            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH] Failed to initialize from storage: {ex.Message}");
            _isInitialized = true; // Mark as initialized even on failure
        }
    }

    private async Task<string?> GetFromStorageAsync(string key)
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }
        catch
        {
            return null;
        }
    }

    private async Task SetInStorageAsync(string key, string? value)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH] Failed to set storage for {key}: {ex.Message}");
        }
    }

    private async Task RemoveFromStorageAsync(string key)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH] Failed to remove from storage: {ex.Message}");
        }
    }

    /// <summary>
    /// Sets the authentication token and persists to localStorage.
    /// Use this for full authentication (login without 2FA or after 2FA verification).
    /// </summary>
    public async Task SetAuthenticationAsync(string? token)
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Setting full authentication with token: {!string.IsNullOrWhiteSpace(token)}");
            _accessToken = token;
            _cachedAuthState = null;
        }

        // Persist to localStorage
        await SetInStorageAsync(TokenKey, token);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        Console.WriteLine($"[AUTH] Full authentication set and state notification sent");
    }

    /// <summary>
    /// Clears all authentication state and storage.
    /// </summary>
    public async Task ClearAsync()
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Clearing authentication state");
            _accessToken = null;
            _cachedAuthState = null;
            _pendingTwoFactorEmail = null;
            _pendingTwoFactorToken = null;
            _pendingTwoFactorType = null;
        }

        // Clear from localStorage
        await RemoveFromStorageAsync(TokenKey);
        await RemoveFromStorageAsync(Pending2FAEmailKey);
        await RemoveFromStorageAsync(Pending2FATokenKey);
        await RemoveFromStorageAsync(Pending2FATypeKey);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        Console.WriteLine($"[AUTH] Authentication cleared");
    }

    /// <summary>
    /// Gets the current access token.
    /// </summary>
    public async Task<string?> GetTokenAsync()
    {
        // Critical fix: If we already have a token in memory, return it immediately
        // This prevents race conditions where we might re-load from storage
        // before the newly set token has been persisted to localStorage
        string? currentToken;
        lock (_stateLock)
        {
            currentToken = _accessToken;
        }
        
        // If token exists in memory, return it immediately
        if (!string.IsNullOrWhiteSpace(currentToken))
        {
            return currentToken;
        }

        // Only initialize from storage if we don't have a token yet
        if (!_isInitialized)
        {
            await InitializeFromStorageAsync();
        }

        lock (_stateLock)
        {
            return _accessToken;
        }
    }

    public void InvalidateCache()
    {
        lock (_stateLock)
        {
            _cachedAuthState = null;
        }
    }

    /// <summary>
    /// Stores pending 2FA session info after initial login when 2FA is required.
    /// This allows the TwoFactorLogin page to complete the authentication.
    /// </summary>
    public async Task SetPendingTwoFactorAsync(string email, string twoFactorToken, string twoFactorType)
    {
        lock (_stateLock)
        {
            Console.WriteLine($"[AUTH] Setting pending 2FA: Email={email}, Type={twoFactorType}");
            _pendingTwoFactorEmail = email;
            _pendingTwoFactorToken = twoFactorToken;
            _pendingTwoFactorType = twoFactorType;
        }

        // Persist to localStorage for page reload survival
        await SetInStorageAsync(Pending2FAEmailKey, email);
        await SetInStorageAsync(Pending2FATokenKey, twoFactorToken);
        await SetInStorageAsync(Pending2FATypeKey, twoFactorType);
    }

    /// <summary>
    /// Retrieves pending 2FA session info for verification.
    /// </summary>
    public async Task<(string? Email, string? Token, string? Type)> GetPendingTwoFactorAsync()
    {
        // Ensure initialized from storage
        if (!_isInitialized)
        {
            await InitializeFromStorageAsync();
        }

        lock (_stateLock)
        {
            return (_pendingTwoFactorEmail, _pendingTwoFactorToken, _pendingTwoFactorType);
        }
    }

    /// <summary>
    /// Clears pending 2FA session info after successful verification or timeout.
    /// </summary>
    public async Task ClearPendingTwoFactorAsync()
    {
        lock (_stateLock)
        {
            Console.WriteLine("[AUTH] Clearing pending 2FA session");
            _pendingTwoFactorEmail = null;
            _pendingTwoFactorToken = null;
            _pendingTwoFactorType = null;
        }

        // Clear from localStorage
        await RemoveFromStorageAsync(Pending2FAEmailKey);
        await RemoveFromStorageAsync(Pending2FATokenKey);
        await RemoveFromStorageAsync(Pending2FATypeKey);
    }

    /// <summary>
    /// Checks if there is a pending 2FA session awaiting verification.
    /// </summary>
    public async Task<bool> HasPendingTwoFactorAsync()
    {
        // Ensure initialized from storage
        if (!_isInitialized)
        {
            await InitializeFromStorageAsync();
        }

        lock (_stateLock)
        {
            return !string.IsNullOrEmpty(_pendingTwoFactorEmail) &&
                   !string.IsNullOrEmpty(_pendingTwoFactorToken);
        }
    }

    // Legacy methods for backward compatibility
    public Task SetTokenAsync(string? token) => SetAuthenticationAsync(token);

    public Task SetTwoFactorVerifiedAsync(bool verified)
    {
        // No longer needed - token presence is sufficient
        Console.WriteLine($"[AUTH] SetTwoFactorVerifiedAsync called (deprecated): {verified}");
        return Task.CompletedTask;
    }

    public Task<bool> IsTwoFactorVerifiedAsync()
    {
        // Always return true if we have a token
        lock (_stateLock)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(_accessToken));
        }
    }
}
