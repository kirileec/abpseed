using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwashbuckleSample.Data;
using Volo.Abp.DependencyInjection;

namespace SwashbuckleSample.EntityFrameworkCore;

public class EntityFrameworkCoreSwashbuckleSampleDbSchemaMigrator
    : ISwashbuckleSampleDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSwashbuckleSampleDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SwashbuckleSampleDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SwashbuckleSampleDbContext>()
            .Database
            .MigrateAsync();
    }
}
