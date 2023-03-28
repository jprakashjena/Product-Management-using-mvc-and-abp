using ProductManagementNew.Localization;
using Volo.Abp.Application.Services;

namespace ProductManagementNew;

/* Inherit your application services from this class.
 */
public abstract class ProductManagementNewAppService : ApplicationService
{
    protected ProductManagementNewAppService()
    {
        LocalizationResource = typeof(ProductManagementNewResource);
    }
}
