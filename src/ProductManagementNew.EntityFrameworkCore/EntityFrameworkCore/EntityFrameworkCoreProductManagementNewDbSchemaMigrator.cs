using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagementNew.Data;
using Volo.Abp.DependencyInjection;

namespace ProductManagementNew.EntityFrameworkCore;

public class EntityFrameworkCoreProductManagementNewDbSchemaMigrator
    : IProductManagementNewDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreProductManagementNewDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ProductManagementNewDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ProductManagementNewDbContext>()
            .Database
            .MigrateAsync();
    }
}
