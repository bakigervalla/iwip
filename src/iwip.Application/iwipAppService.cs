using System;
using System.Collections.Generic;
using System.Text;
using iwip.Localization;
using Volo.Abp.Application.Services;

namespace iwip;

/* Inherit your application services from this class.
 */
public abstract class iwipAppService : ApplicationService
{
    protected iwipAppService()
    {
        LocalizationResource = typeof(iwipResource);
    }
}
