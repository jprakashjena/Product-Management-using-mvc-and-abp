using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NotesModule;

[DependsOn(
    typeof(NotesModuleApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class NotesModuleHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(NotesModuleApplicationContractsModule).Assembly,
            NotesModuleRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<NotesModuleHttpApiClientModule>();
        });
    }
}
