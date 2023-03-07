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

namespace AbpSeed
{
    [DependsOn(typeof(AbpSwashbuckleModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreMySQLModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class App:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;

            //... other configurations.
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.DescribeAllParametersInCamelCase();
                    
                    options.HideAbpEndpoints();
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Abp API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                   
                    
                }
            );
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
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
                options.InjectJavascript("swagger_inject.js");
                options.InjectStylesheet("swagger_theme.css");
                options.EnableFilter();
                options.EnableTryItOutByDefault();
                options.DisplayRequestDuration();
                options.DefaultModelsExpandDepth(-1);
                options.EnableDeepLinking();


            });
            

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(builder =>
            {
                builder.MapGet("/swagger/swagger_inject.js", (context) =>
                {
                    return context.Response.SendFileAsync("swagger_inject.js");
                });
                builder.MapGet("/swagger/swagger_theme.css", (context) =>
                {
                    return context.Response.SendFileAsync("swagger_theme.css");
                });
            });
            app.UseConfiguredEndpoints();
        }
    }
}
