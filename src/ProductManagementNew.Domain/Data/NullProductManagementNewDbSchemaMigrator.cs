using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ProductManagementNew.Data;

/* This is used if database provider does't define
 * IProductManagementNewDbSchemaMigrator implementation.
 */
public class NullProductManagementNewDbSchemaMigrator : IProductManagementNewDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
