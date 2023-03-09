using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SwashbuckleSample.Data;

/* This is used if database provider does't define
 * ISwashbuckleSampleDbSchemaMigrator implementation.
 */
public class NullSwashbuckleSampleDbSchemaMigrator : ISwashbuckleSampleDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
