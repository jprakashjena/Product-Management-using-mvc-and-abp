using NotesModule1.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NotesModule1;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(NotesModule1EntityFrameworkCoreTestModule)
    )]
public class NotesModule1DomainTestModule : AbpModule
{

}
