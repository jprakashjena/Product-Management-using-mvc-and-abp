using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Identity.Features;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Web.Pages.Identity.Components.IdentitySettingGroup;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.Identity.Web.Settings;

public class IdentitySettingManagementPageContributor : SettingPageContributorBase
{
    public IdentitySettingManagementPageContributor()
    {
        RequiredPermissions(IdentityPermissions.SettingManagement);
    }

    public async override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.Identity",
                l["Menu:IdentityManagement"],
                typeof(IdentitySettingGroupViewComponent)
            )
        );

        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
        if (await featureChecker.IsEnabledAsync(IdentityProFeature.EnableLdapLogin))
        {
            context.Groups.Add(
                new SettingPageGroup(
                    "Volo.Abp.Identity.Ldap",
                    l["Menu:IdentityManagement.Ldap"],
                    typeof(IdentitySettingLdapGroupViewComponent)
                )
            );
        }

        if (await featureChecker.IsEnabledAsync(IdentityProFeature.EnableOAuthLogin))
        {
            context.Groups.Add(
                new SettingPageGroup(
                    "Volo.Abp.Identity.OAuth",
                    l["Menu:IdentityManagement.OAuth"],
                    typeof(IdentitySettingOAuthGroupViewComponent)
                )
            );
        }
    }
}
