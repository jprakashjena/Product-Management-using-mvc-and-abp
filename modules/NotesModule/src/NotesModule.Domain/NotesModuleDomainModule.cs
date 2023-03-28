using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NotesModule;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(NotesModuleDomainSharedModule)
)]
public class NotesModuleDomainModule : AbpModule
{

}
