using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace iwip.Data;

/* This is used if database provider does't define
 * IiwipDbSchemaMigrator implementation.
 */
public class NulliwipDbSchemaMigrator : IiwipDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
