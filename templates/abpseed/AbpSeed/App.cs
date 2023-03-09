
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Volo.Abp.Caching;
using App.EFCore.MySQL;
using App.SwaggerUI.Fix;
using Microsoft.OpenApi.Models;

namespace AbpSeed
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule), //ÒÀÀµ×¢Èë
        typeof(AbpEntityFrameworkCoreMySQLModule), //mysql EFCore
        typeof(AbpSwashbuckleModuleFixed), // ×îÐÂ°æ±¾swagger-ui
        typeof(AbpCachingStackExchangeRedisModule), //redis»º´æ
        typeof(AbpCachingModule) //»º´æÄ£¿é
        )]
    public class App:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddAbpDbContext<MySQLDBContext>();

            //... other configurations.
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.DescribeAllParametersInCamelCase();
                    options.HideAbpEndpoints();
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Abp API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName.Replace("+","."));
                }
            );
            Configure<AbpDbContextOptions>(options =>
            {    
                options.Configure<MySQLDBContext>(options =>
                {
                    options.UseMySQL();
                });
            });
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "App";
            });

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");  
            }
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Abp API");
                options.InjectStylesheet("swagger_theme.css");
                options.EnableFilter();
                //options.EnableTryItOutByDefault();
                options.DisplayRequestDuration();
                //options.DefaultModelsExpandDepth(-1);
                //options.EnableDeepLinking();
            });
            

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(builder =>
            {
                builder.MapGet("/swagger/swagger_theme.css", (context) =>
                {
                    return context.Response.SendFileAsync("swagger_theme.css");
                });
            });
            app.UseConfiguredEndpoints();
        }
    }
}
