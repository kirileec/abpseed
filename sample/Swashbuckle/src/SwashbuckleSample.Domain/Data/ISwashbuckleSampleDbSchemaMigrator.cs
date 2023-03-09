using System.Threading.Tasks;

namespace SwashbuckleSample.Data;

public interface ISwashbuckleSampleDbSchemaMigrator
{
    Task MigrateAsync();
}
