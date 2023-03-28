using NotesModule.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace NotesModule.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class NotesModulePageModel : AbpPageModel
{
    protected NotesModulePageModel()
    {
        LocalizationResourceType = typeof(NotesModuleResource);
        ObjectMapperContext = typeof(NotesModuleWebModule);
    }
}
