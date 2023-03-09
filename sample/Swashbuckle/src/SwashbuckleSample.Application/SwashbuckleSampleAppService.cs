using System;
using System.Collections.Generic;
using System.Text;
using SwashbuckleSample.Localization;
using Volo.Abp.Application.Services;

namespace SwashbuckleSample;

/* Inherit your application services from this class.
 */
public abstract class SwashbuckleSampleAppService : ApplicationService
{
    protected SwashbuckleSampleAppService()
    {
        LocalizationResource = typeof(SwashbuckleSampleResource);
    }
}
