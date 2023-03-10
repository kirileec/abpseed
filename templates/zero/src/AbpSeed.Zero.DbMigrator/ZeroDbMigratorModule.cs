using AbpSeed.Zero.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpSeed.Zero.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ZeroEntityFrameworkCoreModule),
    typeof(ZeroApplicationContractsModule)
    )]
public class ZeroDbMigratorModule : AbpModule
{

}
