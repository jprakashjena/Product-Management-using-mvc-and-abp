using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace NotesModule1;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NotesModule1HttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class NotesModule1ConsoleApiClientModule : AbpModule
{

}
