using System.Threading.Tasks;

namespace AbpSeed.Zero.Data;

public interface IZeroDbSchemaMigrator
{
    Task MigrateAsync();
}
