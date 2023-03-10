
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Volo.Abp.Caching;

using App.SwaggerUI.Fix;
using Microsoft.OpenApi.Models;
using App.AbpSeed.Main;

namespace AbpSeed
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
       
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreMySQLModule), //mysql EFCore
        typeof(AbpSwashbuckleModuleFixed),
         typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule), 
        typeof(AbpCachingModule)

        )]
    public class App:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            context.Services.AddAbpDbContext<MySQLDBContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });
            context.Services.AddAbpDbContext<MyDBContext>();
            
            //... other configurations.
            services.AddAbpSwaggerGen(
                options =>
                {
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
                options.Configure<MyDBContext>(options =>
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
                options.EnableFilter();
                options.EnableTryItOutByDefault();
                options.DisplayRequestDuration();

            });

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(builder =>
            {

            });
            app.UseConfiguredEndpoints();
        }
    }
}
