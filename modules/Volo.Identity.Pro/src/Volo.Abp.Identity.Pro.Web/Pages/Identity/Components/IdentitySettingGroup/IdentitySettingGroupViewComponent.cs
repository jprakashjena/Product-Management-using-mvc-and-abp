using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Features;


namespace Volo.Abp.Identity.Web.Pages.Identity.Components.IdentitySettingGroup;

public class IdentitySettingGroupViewComponent : AbpViewComponent
{
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; }
    protected IFeatureChecker FeatureChecker { get; }

    public IdentitySettingGroupViewComponent(IIdentitySettingsAppService identitySettingsAppService, IFeatureChecker featureChecker)
    {
        ObjectMapperContext = typeof(AbpIdentityWebModule);

        IdentitySettingsAppService = identitySettingsAppService;
        FeatureChecker = featureChecker;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var settingsViewModel = new IdentitySettingViewModel
        {
            IdentitySettings = await IdentitySettingsAppService.GetAsync()
        };

        return View("~/Pages/Identity/Components/IdentitySettingGroup/Default.cshtml", settingsViewModel);
    }

    public class IdentitySettingViewModel
    {
        public IdentitySettingsDto IdentitySettings { get; set; }
    }
}
