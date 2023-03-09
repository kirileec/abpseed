using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace AbpSeed.Abp.Swashbuckle
{
    [DependsOn(
        typeof(AbpSwashbuckleModule)
        )]
    public class AbpSeedSwashbuckleModule : AbpModule
    {

        // public override void ConfigureServices(ServiceConfigurationContext context)
        // {
        //     Configure<AbpVirtualFileSystemOptions>(options =>
        //     {
        //         options.FileSets.AddEmbedded<AbpSeedSwashbuckleModule>();
        //     });
        // }
    }
}
