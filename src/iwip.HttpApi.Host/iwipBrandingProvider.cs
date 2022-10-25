using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace iwip;

[Dependency(ReplaceServices = true)]
public class iwipBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "iwip";
}
