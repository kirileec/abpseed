using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SwashbuckleSample.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SwashbuckleSampleDbContextFactory : IDesignTimeDbContextFactory<SwashbuckleSampleDbContext>
{
    public SwashbuckleSampleDbContext CreateDbContext(string[] args)
    {
        SwashbuckleSampleEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        //var connectionString = @$"Data Source={Path.Join(AppContext.BaseDirectory, "SwashbuckleSample.db")}";
        var builder = new DbContextOptionsBuilder<SwashbuckleSampleDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        return new SwashbuckleSampleDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
          //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SwashbuckleSample.DbMigrator/"))
          .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
        //var builder = new ConfigurationBuilder()
        //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SwashbuckleSample.DbMigrator/"))
        //    .AddJsonFile("appsettings.json", optional: false);

        //return builder.Build();
    }
}
