using Volo.Abp.Modularity;

namespace ProductManagementNew;

[DependsOn(
    typeof(ProductManagementNewApplicationModule),
    typeof(ProductManagementNewDomainTestModule)
    )]
public class ProductManagementNewApplicationTestModule : AbpModule
{

}
