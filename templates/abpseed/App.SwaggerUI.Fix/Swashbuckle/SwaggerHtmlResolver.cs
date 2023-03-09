using System.IO;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Swashbuckle;

namespace AbpSeed.Abp.Swashbuckle
{
    /// <summary>
    /// ����Abp����������õ� Swashbuckle.AspNetCore.SwaggerUI �汾Ϊ 6.3.0���޸���api��ַ���ܣ��滻ISwaggerHtmlResolver��ʵ��ʹ���°汾SwaggerUI��Ŀ��
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