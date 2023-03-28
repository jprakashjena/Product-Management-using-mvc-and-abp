using NotesModule1.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NotesModule1;

public abstract class NotesModule1Controller : AbpControllerBase
{
    protected NotesModule1Controller()
    {
        LocalizationResource = typeof(NotesModule1Resource);
    }
}
