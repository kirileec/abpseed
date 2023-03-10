using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Swagger;

namespace Volo.Abp.Swagger.Microsoft.AspNetCore.Builder
{
    public static class AbpSwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseAbpSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = null)
        {
            var resolver = app.ApplicationServices.GetService<ISwaggerHtmlResolver>();

            return app.UseSwaggerUI(options =>
            {
                options.InjectJavascript("abp.js");
                options.InjectJavascript("abp.swagger.js");
                options.IndexStream = () => resolver.Resolver();

                setupAction?.Invoke(options);
            });
        }
    }
}
