using AbpSeed.Zero.Localization;
using Localization.Resources.AbpUi;
using SharpAbp.Abp.Account;
using SharpAbp.Abp.AuditLogging;
using SharpAbp.Abp.Identity;
using SharpAbp.Abp.OpenIddict;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace AbpSeed.Zero;

[DependsOn(
    typeof(ZeroApplicationContractsModule),
    typeof(AuditLoggingHttpApiModule),
    typeof(AccountHttpApiModule),
    typeof(IdentityHttpApiModule),
    typeof(OpenIddictHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
public class ZeroHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ZeroResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
