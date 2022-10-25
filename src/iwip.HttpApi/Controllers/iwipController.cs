using iwip.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace iwip.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class iwipController : AbpControllerBase
{
    protected iwipController()
    {
        LocalizationResource = typeof(iwipResource);
    }
}
