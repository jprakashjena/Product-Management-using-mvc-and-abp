using NotesModule.Notes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NotesModule.EntityFrameworkCore;

[DependsOn(
    typeof(NotesModuleDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class NotesModuleEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<NotesModuleDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<Note, Notes.EfCoreNoteRepository>();

        });
    }
}