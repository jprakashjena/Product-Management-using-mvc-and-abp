using ProductManagementNew.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ProductManagementNew;

[DependsOn(
    typeof(ProductManagementNewEntityFrameworkCoreTestModule)
    )]
public class ProductManagementNewDomainTestModule : AbpModule
{

}
