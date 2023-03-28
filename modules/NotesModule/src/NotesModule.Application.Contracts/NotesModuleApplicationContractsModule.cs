using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NotesModule;

[DependsOn(
    typeof(NotesModuleDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class NotesModuleApplicationContractsModule : AbpModule
{

}
