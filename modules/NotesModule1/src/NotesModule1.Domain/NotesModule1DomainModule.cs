using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NotesModule1;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(NotesModule1DomainSharedModule)
)]
public class NotesModule1DomainModule : AbpModule
{

}
