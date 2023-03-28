using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace NotesModule;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NotesModuleHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class NotesModuleConsoleApiClientModule : AbpModule
{

}
