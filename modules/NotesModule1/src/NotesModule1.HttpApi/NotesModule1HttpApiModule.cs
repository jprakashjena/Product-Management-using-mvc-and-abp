using Localization.Resources.AbpUi;
using NotesModule1.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace NotesModule1;

[DependsOn(
    typeof(NotesModule1ApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class NotesModule1HttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(NotesModule1HttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<NotesModule1Resource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
