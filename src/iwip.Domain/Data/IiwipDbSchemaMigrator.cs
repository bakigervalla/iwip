using System.Threading.Tasks;

namespace iwip.Data;

public interface IiwipDbSchemaMigrator
{
    Task MigrateAsync();
}
