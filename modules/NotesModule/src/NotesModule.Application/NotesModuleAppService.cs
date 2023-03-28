using NotesModule.Localization;
using Volo.Abp.Application.Services;

namespace NotesModule;

public abstract class NotesModuleAppService : ApplicationService
{
    protected NotesModuleAppService()
    {
        LocalizationResource = typeof(NotesModuleResource);
        ObjectMapperContext = typeof(NotesModuleApplicationModule);
    }
}
