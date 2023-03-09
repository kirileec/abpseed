using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Swagger
{
    [DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAspNetCoreMvcModule))]
    public class AbpSwashbuckleModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpSwashbuckleModule>();
            });
        }
    }
}
