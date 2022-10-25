using iwip.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace iwip.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(iwipMongoDbModule),
    typeof(iwipApplicationContractsModule)
    )]
public class iwipDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
