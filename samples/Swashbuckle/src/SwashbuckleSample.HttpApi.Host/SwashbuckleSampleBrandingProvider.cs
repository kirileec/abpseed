using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SwashbuckleSample;

[Dependency(ReplaceServices = true)]
public class SwashbuckleSampleBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SwashbuckleSample";
}
