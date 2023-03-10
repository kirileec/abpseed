using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace App.AbpSeed.Main
{
    [ConnectionStringName("Default")]
    public class MyDBContext : AbpDbContext<MyDBContext>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }
    }
}
