using Microsoft.JSInterop;

namespace AuthManagement.Services;

public class SweetAlertService
{
    private readonly IJSRuntime _jsRuntime;

    public SweetAlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> ConfirmDeleteAsync(string title = "Are you sure?", string text = "You won't be able to revert this!")
    {
        try
        {
            return await _jsRuntime.InvokeAsync<bool>("showDeleteConfirmation", title, text);
        }
        catch
        {
            return false;
        }
    }

    public async Task ShowSuccessToastAsync(string message)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("showSuccessToast", message);
        }
        catch
        {
            // Ignore errors
        }
    }

    public async Task ShowErrorToastAsync(string message)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("showErrorToast", message);
        }
        catch
        {
            // Ignore errors
        }
    }

    public async Task ShowWarningToastAsync(string message)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("showWarningToast", message);
        }
        catch
        {
            // Ignore errors
        }
    }

    public async Task ShowInfoToastAsync(string message)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("showInfoToast", message);
        }
        catch
        {
            // Ignore errors
        }
    }
}
