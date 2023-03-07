using Volo.Abp.Modularity;

namespace AbpSeed
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseAutofac();
            await builder.AddApplicationAsync<App>();

            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            await app.InitializeApplicationAsync();
            await app.RunAsync();
        }
    }
}