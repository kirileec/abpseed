using System;
using System.Collections.Generic;
using System.Text;
using AbpSeed.Zero.Localization;
using Volo.Abp.Application.Services;

namespace AbpSeed.Zero;

/* Inherit your application services from this class.
 */
public abstract class ZeroAppService : ApplicationService
{
    protected ZeroAppService()
    {
        LocalizationResource = typeof(ZeroResource);
    }
}
