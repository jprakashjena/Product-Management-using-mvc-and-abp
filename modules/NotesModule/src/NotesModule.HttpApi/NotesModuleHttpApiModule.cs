using Localization.Resources.AbpUi;
using NotesModule.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NotesModule;

[DependsOn(
    typeof(NotesModuleApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class NotesModuleHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(NotesModuleHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<NotesModuleResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
