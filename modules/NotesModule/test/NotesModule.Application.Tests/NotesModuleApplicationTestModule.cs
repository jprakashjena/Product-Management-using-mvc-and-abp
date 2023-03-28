using Volo.Abp.Modularity;

namespace NotesModule;

[DependsOn(
    typeof(NotesModuleApplicationModule),
    typeof(NotesModuleDomainTestModule)
    )]
public class NotesModuleApplicationTestModule : AbpModule
{

}
