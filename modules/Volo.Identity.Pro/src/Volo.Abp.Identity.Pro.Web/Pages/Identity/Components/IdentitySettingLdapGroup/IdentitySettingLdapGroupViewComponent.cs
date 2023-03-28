using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Features;
using Volo.Abp.Identity.Features;

namespace Volo.Abp.Identity.Web.Pages.Identity.Components.IdentitySettingGroup;

public class IdentitySettingLdapGroupViewComponent : AbpViewComponent
{
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; }
    protected IFeatureChecker FeatureChecker { get; }

    public IdentitySettingLdapGroupViewComponent(IIdentitySettingsAppService identitySettingsAppService, IFeatureChecker featureChecker)
    {
        ObjectMapperContext = typeof(AbpIdentityWebModule);

        IdentitySettingsAppService = identitySettingsAppService;
        FeatureChecker = featureChecker;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var settingsViewModel = new IdentityLdapSettingViewModel();

        if (await FeatureChecker.IsEnabledAsync(IdentityProFeature.EnableLdapLogin))
        {
            settingsViewModel.IdentityLdapSettings = await IdentitySettingsAppService.GetLdapAsync();
        }

        return View("~/Pages/Identity/Components/IdentitySettingLdapGroup/Default.cshtml", settingsViewModel);
    }

    public class IdentityLdapSettingViewModel
    {
        public IdentityLdapSettingsDto IdentityLdapSettings { get; set; }
    }
}
