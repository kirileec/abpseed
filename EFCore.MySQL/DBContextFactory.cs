using AbpSeed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.MySQL
{
    public class DBContextFactory : IDesignTimeDbContextFactory<DBContext>
    {
        public DBContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<DBContext>()
                .UseMySql(configuration.GetConnectionString("MySQLConnection"),ServerVersion.AutoDetect(configuration.GetConnectionString("MySQLConnection")));

            return new DBContext(builder.Options);
        }
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpSeed/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
