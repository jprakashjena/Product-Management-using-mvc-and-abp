using ProductManagementNew.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ProductManagementNew.Web.Pages;

public abstract class ProductManagementNewPageModel : AbpPageModel
{
    protected ProductManagementNewPageModel()
    {
        LocalizationResourceType = typeof(ProductManagementNewResource);
    }
}
