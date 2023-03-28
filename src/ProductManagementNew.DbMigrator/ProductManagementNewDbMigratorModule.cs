using ProductManagementNew.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ProductManagementNew.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ProductManagementNewEntityFrameworkCoreModule),
    typeof(ProductManagementNewApplicationContractsModule)
)]
public class ProductManagementNewDbMigratorModule : AbpModule
{

}
