using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Admin.Web.Pages.Account.Components.AccountSettingExternalProviderGroup;
using Volo.Abp.Account.Admin.Web.Pages.Account.Components.AccountSettingGroup;
using Volo.Abp.Account.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.Account.Admin.Web.Settings;

public class AccountSettingManagementPageContributor : SettingPageContributorBase
{
    public AccountSettingManagementPageContributor()
    {
        RequiredPermissions(AccountPermissions.SettingManagement);
    }

    public async override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.Account",
                l["Menu:Account"],
                typeof(AccountSettingGroupViewComponent)
            )
        );

        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var accountExternalProviderSettingsDto = await context.ServiceProvider.GetRequiredService<IAccountSettingsAppService>().GetExternalProviderAsync();
        if ((!currentTenant.IsAvailable && accountExternalProviderSettingsDto.Settings.Any())
            || (currentTenant.IsAvailable && accountExternalProviderSettingsDto.Settings.Any(x => x.Enabled)))
        {
            context.Groups.Add(
                new SettingPageGroup(
                    "Volo.Abp.Account.ExternalProvider",
                    l["Menu:Account.ExternalProvider"],
                    typeof(AccountSettingExternalProviderViewComponent)
                )
            );
        }
    }
}
