using iwip.MongoDB;
using Volo.Abp.Modularity;

namespace iwip;

[DependsOn(
    typeof(iwipMongoDbTestModule)
    )]
public class iwipDomainTestModule : AbpModule
{

}
