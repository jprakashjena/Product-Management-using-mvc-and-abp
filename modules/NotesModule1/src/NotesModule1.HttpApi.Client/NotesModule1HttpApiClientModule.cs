using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NotesModule1;

[DependsOn(
    typeof(NotesModule1ApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class NotesModule1HttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(NotesModule1ApplicationContractsModule).Assembly,
            NotesModule1RemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<NotesModule1HttpApiClientModule>();
        });
    }
}
