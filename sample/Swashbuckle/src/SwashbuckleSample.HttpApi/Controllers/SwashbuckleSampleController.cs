using SwashbuckleSample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SwashbuckleSample.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SwashbuckleSampleController : AbpControllerBase
{
    protected SwashbuckleSampleController()
    {
        LocalizationResource = typeof(SwashbuckleSampleResource);
    }
}
