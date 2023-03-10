using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Modularity;

namespace AbpSeed.Abp.Swashbuckle.Versioning
{
    public class AbpSeedSwashbuckleVersioningModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpSeedSwashbuckleVersioningOptions>(options =>
            {
                options.Title = "AbpSeed";
            });
            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}
