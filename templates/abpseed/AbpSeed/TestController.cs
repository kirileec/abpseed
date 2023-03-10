using App.EFCore.MySQL;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace App.AbpSeed.Main
{
    [ApiController]
    [Route("[controller]")]
    public class TestController:AbpController
    {
        private readonly MySQLDBContext _dbContext;
        public TestController(MySQLDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet("index")]
        public ActionResult Index()
        {
            return new JsonResult("ddd");
        }
    }
}
