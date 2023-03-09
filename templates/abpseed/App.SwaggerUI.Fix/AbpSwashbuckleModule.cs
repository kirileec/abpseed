using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace App.SwaggerUI.Fix
{
    [DependsOn(
         typeof(AbpSwashbuckleModule)
         )]
    public class AbpSwashbuckleModuleFixed : AbpModule
    {
    }
}
