using Volo.Abp.Modularity;

namespace NotesModule1;

[DependsOn(
    typeof(NotesModule1ApplicationModule),
    typeof(NotesModule1DomainTestModule)
    )]
public class NotesModule1ApplicationTestModule : AbpModule
{

}
