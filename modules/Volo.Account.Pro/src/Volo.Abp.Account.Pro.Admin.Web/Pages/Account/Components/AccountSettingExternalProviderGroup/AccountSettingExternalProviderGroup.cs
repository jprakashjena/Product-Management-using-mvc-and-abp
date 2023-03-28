using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account.Admin.Web.Pages.Account.Components.AccountSettingExternalProviderGroup;

public class AccountSettingExternalProviderViewComponent : AbpViewComponent
{
    public AccountSettingsExternalProviderViewModel SettingsViewModel { get; set; }

    protected IAccountSettingsAppService AccountSettingsAppService { get; }

    public AccountSettingExternalProviderViewComponent(IAccountSettingsAppService accountSettingsAppService)
    {
        AccountSettingsAppService = accountSettingsAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        SettingsViewModel = new AccountSettingsExternalProviderViewModel
        {
            AccountExternalProviderSettings = await AccountSettingsAppService.GetExternalProviderAsync()
        };

        return View("~/Pages/Account/Components/AccountSettingExternalProviderGroup/Default.cshtml", SettingsViewModel);
    }

    public class AccountSettingsExternalProviderViewModel
    {
        public AccountExternalProviderSettingsDto AccountExternalProviderSettings { get; set; }
    }
}
