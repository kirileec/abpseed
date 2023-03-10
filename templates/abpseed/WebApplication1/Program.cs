namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseAutofac();
            await builder.AddApplicationAsync<App>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            await app.InitializeApplicationAsync();
            await app.RunAsync();
        }
    }
}