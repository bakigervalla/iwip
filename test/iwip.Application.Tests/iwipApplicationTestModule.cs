using Volo.Abp.Modularity;

namespace iwip;

[DependsOn(
    typeof(iwipApplicationModule),
    typeof(iwipDomainTestModule)
    )]
public class iwipApplicationTestModule : AbpModule
{

}
