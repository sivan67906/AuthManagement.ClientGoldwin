using System;
using System.Threading.Tasks;

namespace AuthManagement.Services
{
    public class BootstrapDialogService
    {
        public event Func<string, string, Task<bool>>? OnShowConfirm;

        public async Task<bool> ShowConfirmAsync(string title, string message)
        {
            if (OnShowConfirm != null)
            {
                return await OnShowConfirm.Invoke(title, message);
            }
            return false;
        }
    }
}
