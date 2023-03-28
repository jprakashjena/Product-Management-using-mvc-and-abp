using NotesModule1.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace NotesModule1.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class NotesModule1PageModel : AbpPageModel
{
    protected NotesModule1PageModel()
    {
        LocalizationResourceType = typeof(NotesModule1Resource);
        ObjectMapperContext = typeof(NotesModule1WebModule);
    }
}
