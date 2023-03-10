using AbpSeed.Abp.Swashbuckle;
using AbpSeed.Abp.Swashbuckle.Versioning;
using AbpSeed.Zero.EntityFrameworkCore;
using AbpSeed.Zero.MultiTenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using SharpAbp.Abp.Account;
using SharpAbp.Abp.AspNetCore;
using SharpAbp.Abp.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace AbpSeed.Zero;

[DependsOn(
    typeof(ZeroHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(ZeroApplicationModule),
    typeof(ZeroEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(SharpAbpAspNetCoreModule),
    typeof(SharpAbpAspNetCoreMvcModule),
    typeof(AbpSeedSwashbuckleModule),
    typeof(AbpSeedSwashbuckleVersioningModule)
)]
public class ZeroHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            PreConfigure<OpenIddictServerBuilder>(options =>
            {
                options
                    .UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .DisableTransportSecurityRequirement();
            });

            builder.AddValidation(options =>
            {
                options.AddAudiences("Zero");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        //配置Json
        ConfigureJsonOptions();

        //配置Api版本
        ConfigureApiVersion(context);

        //配置跨域
        ConfigureCors(context, configuration);

        //配置认证
        ConfigureAuthentication(context, configuration);

        //配置js,css绑定
        ConfigureBundles();

        //配置url数据,前端,站点等地址
        ConfigureUrls(configuration);

        //动态从Application层生成控制器,部分HttpApi已经引用的dll不需要开启
        //ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);

        //配置Swagger
        ConfigureSwaggerServices(context, configuration);

        //主要解决Swagger登录的时候,Chrome的一些配置导致无法登录等问题
        context.AddSameSiteCookiePolicy();
        context.AddDisableAntiForgery();
    }

    /// <summary>
    /// 配置Json
    /// </summary>
    private void ConfigureJsonOptions()
    {
        Configure<AbpJsonOptions>(options =>
        {
            options.InputDateTimeFormats.Add("yyyy-MM-dd HH:mm:ss");
            options.InputDateTimeFormats.Add("yyyy-MM-dd");
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });
    }

    /// <summary>
    /// 配置认证
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<AbpAuthorizationExceptionHandlerOptions>(options =>
        {
            options.AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        context.Services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "Zero";
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                options.TokenValidationParameters.ValidateIssuer = Convert.ToBoolean(configuration["AuthServer:ValidateIssuer"]);

                options.TokenValidationParameters.ValidIssuers = configuration["AuthServer:ValidIssuers"]
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(o => o.RemovePostFix("/"))
                    .ToArray();
            });

        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ZeroDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpSeed.Zero.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<ZeroDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpSeed.Zero.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<ZeroApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpSeed.Zero.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<ZeroApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AbpSeed.Zero.Application"));
            });
        }
    }

    /// <summary>
    /// 动态创建Application对应的控制器
    /// </summary>
    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(ZeroApplicationModule).Assembly);
        });
    }

    /// <summary>
    /// 配置Api版本
    /// </summary>
    /// <param name="context"></param>
    private void ConfigureApiVersion(ServiceConfigurationContext context)
    {
        context.Services.AddAbpApiVersioning(options =>
        {
            // Show neutral/versionless APIs.
            options.UseApiBehavior = false;
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        context.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ChangeControllerModelApiExplorerGroupName = false;
        });
    }


    /// <summary>
    /// 配置swagger
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    private void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<AbpSeedSwashbuckleVersioningOptions>(options =>
        {
            options.Title = "AbpSeed";
        });

        var displaySwagger = bool.Parse(configuration["App:DisplaySwagger"]);
        if (displaySwagger)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                        {"Zero", "Zero API"}
                },
                options =>
                {
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.CustomSchemaIds(type => type.FullName);
                });
        }
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                var allowAnyCors = bool.Parse(configuration["App:AllowAnyCors"]);
                if (allowAnyCors)
                {
                    builder
                      .WithOrigins("*")
                      .WithAbpExposedHeaders()
                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                }
                else
                {
                    builder
                      .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                      )
                      .WithAbpExposedHeaders()
                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
                }
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();

        var displaySwagger = bool.Parse(configuration["App:DisplaySwagger"]);
        if (displaySwagger)
        {
            app.UseSwagger();
            app.UseAbpSeedSwaggerUI(options =>
            {
                var provider = context.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                options.OAuthScopes("Admin");
            });
        }

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
