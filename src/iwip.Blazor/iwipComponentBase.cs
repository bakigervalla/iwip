using iwip.Localization;
using Volo.Abp.AspNetCore.Components;

namespace iwip.Blazor;

public abstract class iwipComponentBase : AbpComponentBase
{
    protected iwipComponentBase()
    {
        LocalizationResource = typeof(iwipResource);
    }
}
