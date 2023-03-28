using System.Threading.Tasks;

namespace ProductManagementNew.Data;

public interface IProductManagementNewDbSchemaMigrator
{
    Task MigrateAsync();
}
