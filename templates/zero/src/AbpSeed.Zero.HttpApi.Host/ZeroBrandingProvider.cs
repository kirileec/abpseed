using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AbpSeed.Zero;

[Dependency(ReplaceServices = true)]
public class ZeroBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Zero";
}
