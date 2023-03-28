using NotesModule.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NotesModule;

public abstract class NotesModuleController : AbpControllerBase
{
    protected NotesModuleController()
    {
        LocalizationResource = typeof(NotesModuleResource);
    }
}
