using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ProductManagementNew.Web;

[Dependency(ReplaceServices = true)]
public class ProductManagementNewBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ProductManagementNew";
}
