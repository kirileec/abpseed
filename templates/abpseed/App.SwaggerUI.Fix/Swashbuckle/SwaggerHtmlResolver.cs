using System.IO;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Swashbuckle;

namespace AbpSeed.Abp.Swashbuckle
{
    /// <summary>
    /// 由于Abp框架内置引用的 Swashbuckle.AspNetCore.SwaggerUI 版本为 6.3.0，无复制api地址功能，替换ISwaggerHtmlResolver来实现使用新版本SwaggerUI的目的
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ISwaggerHtmlResolver))]
    public class AbpSeedSwaggerHtmlResolver : ISwaggerHtmlResolver, ITransientDependency
    {
        public virtual Stream Resolver()
        {
            var stream = typeof(SwaggerUIOptions).GetTypeInfo().Assembly
                .GetManifestResourceStream("Swashbuckle.AspNetCore.SwaggerUI.index.html");

            var html = new StreamReader(stream)
                .ReadToEnd()
                .Replace("SwaggerUIBundle(configObject)", "abp.SwaggerUIBundle(configObject)");

            return new MemoryStream(Encoding.UTF8.GetBytes(html));
        }
    }
}