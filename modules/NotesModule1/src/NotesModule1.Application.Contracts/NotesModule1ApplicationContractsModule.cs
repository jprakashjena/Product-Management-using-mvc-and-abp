using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NotesModule1;

[DependsOn(
    typeof(NotesModule1DomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class NotesModule1ApplicationContractsModule : AbpModule
{

}
