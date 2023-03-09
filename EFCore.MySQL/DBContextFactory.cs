
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.EFCore.MySQL
{
    public class DBContextFactory : IDesignTimeDbContextFactory<MySQLDBContext>
    {
        public MySQLDBContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<MySQLDBContext>()
                .UseMySql(configuration.GetConnectionString("MySQLConnection"),ServerVersion.AutoDetect(configuration.GetConnectionString("MySQLConnection")));

            return new MySQLDBContext(builder.Options);
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
