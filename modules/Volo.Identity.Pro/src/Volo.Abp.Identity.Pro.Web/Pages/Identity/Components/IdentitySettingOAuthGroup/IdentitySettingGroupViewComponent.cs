using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Features;
using Volo.Abp.Identity.Features;

namespace Volo.Abp.Identity.Web.Pages.Identity.Components.IdentitySettingGroup;

public class IdentitySettingOAuthGroupViewComponent : AbpViewComponent
{
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; }
    protected IFeatureChecker FeatureChecker { get; }

    public IdentitySettingOAuthGroupViewComponent(IIdentitySettingsAppService identitySettingsAppService, IFeatureChecker featureChecker)
    {
        ObjectMapperContext = typeof(AbpIdentityWebModule);

        IdentitySettingsAppService = identitySettingsAppService;
        FeatureChecker = featureChecker;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var settingsViewModel = new IdentitySettingOAuthViewModel();

        if (await FeatureChecker.IsEnabledAsync(IdentityProFeature.EnableOAuthLogin))
        {
            settingsViewModel.IdentityOAuthSettings = await IdentitySettingsAppService.GetOAuthAsync();
        }

        return View("~/Pages/Identity/Components/IdentitySettingOAuthGroup/Default.cshtml", settingsViewModel);
    }

    public class IdentitySettingOAuthViewModel
    {
        public IdentityOAuthSettingsDto IdentityOAuthSettings { get; set; }
    }
}
