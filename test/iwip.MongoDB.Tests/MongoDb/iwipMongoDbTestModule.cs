using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace iwip.MongoDB;

[DependsOn(
    typeof(iwipTestBaseModule),
    typeof(iwipMongoDbModule)
    )]
public class iwipMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = iwipMongoDbFixture.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });
    }
}
