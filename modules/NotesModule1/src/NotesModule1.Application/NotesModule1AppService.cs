using NotesModule1.Localization;
using Volo.Abp.Application.Services;

namespace NotesModule1;

public abstract class NotesModule1AppService : ApplicationService
{
    protected NotesModule1AppService()
    {
        LocalizationResource = typeof(NotesModule1Resource);
        ObjectMapperContext = typeof(NotesModule1ApplicationModule);
    }
}
