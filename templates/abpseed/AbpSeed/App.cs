
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.Swagger;
using Volo.Abp.Swagger.Microsoft.AspNetCore.Builder;
using Volo.Abp.Swagger.Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Volo.Abp.Caching;
using App.EFCore.MySQL;

namespace AbpSeed
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
        
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpCachingModule)
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
                    //options.IncludeXmlComments("App.Entities.xml");
                    //options.IncludeXmlComments("App.Models.xml");

                }
            );
            Configure<AbpDbContextOptions>(options =>
            {
                
                options.Configure<MySQLDBContext>(options =>
                {
                    options.UseMySQL();
                });
            });
            Configure<RedisCacheOptions>(options =>
            {

            });
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "App";
            });

            //Configure<AbpDbConnectionOptions>(options =>
            //{
            //    options.ConnectionStrings.Default = "...";
            //    options.ConnectionStrings["MySQLConnection"] = "...";
            //});
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
                //policy.AllowCredentials();
            });

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                
            }
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
               
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Abp API");
     
                //options.InjectJavascript("abp.js");
                //options.InjectJavascript("abp.swagger.js");
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
                //builder.MapGet("/swagger/swagger_inject.js", (context) =>
                //{
                //    return context.Response.SendFileAsync("swagger_inject.js");
                //});
                builder.MapGet("/swagger/swagger_theme.css", (context) =>
                {
                    return context.Response.SendFileAsync("swagger_theme.css");
                });
                builder.MapGet("/swagger/abp.js", (context) =>
                {
                    return context.Response.SendFileAsync("abp.js");
                });
                builder.MapGet("/swagger/abp.swagger.js", (context) =>
                {
                    return context.Response.SendFileAsync("abp.swagger.js");
                });
            });
            app.UseConfiguredEndpoints();
        }
    }
}