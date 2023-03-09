using SwashbuckleSample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SwashbuckleSample.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SwashbuckleSampleEntityFrameworkCoreModule),
    typeof(SwashbuckleSampleApplicationContractsModule)
    )]
public class SwashbuckleSampleDbMigratorModule : AbpModule
{

}
