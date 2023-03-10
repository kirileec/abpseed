using AbpSeed.Zero.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpSeed.Zero.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ZeroController : AbpControllerBase
{
    protected ZeroController()
    {
        LocalizationResource = typeof(ZeroResource);
    }
}
