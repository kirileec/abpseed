using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Swagger;

namespace Volo.Abp.Swagger.Microsoft.Extensions.DependencyInjection
{
    public static class AbpSwaggerGenOptionsExtensions
    {
        public static void HideAbpEndpoints(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.DocumentFilter<AbpSwashbuckleDocumentFilter>();
        }
    }
}
