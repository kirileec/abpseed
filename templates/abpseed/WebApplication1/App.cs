using App.SwaggerUI.Fix;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace WebApplication1
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreMySQLModule))]
    [DependsOn(typeof(AbpSwashbuckleModuleFixed))]
    [DependsOn(typeof(AbpAutofacModule))]
    public class App:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context); 
        }
    }
}
