using ProductManagementNew.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ProductManagementNew.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ProductManagementNewController : AbpControllerBase
{
    protected ProductManagementNewController()
    {
        LocalizationResource = typeof(ProductManagementNewResource);
    }
}
