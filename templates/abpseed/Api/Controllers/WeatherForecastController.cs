using App.Api;
using App.Contracts;
using App.EFCore.MySQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    /// <summary>
    /// WeatherForecastController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private readonly MySQLDBContext _dbContext;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceProvider context)
        {
            _dbContext = context.GetRequiredService<MySQLDBContext>();
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 测试Json返回参数
        /// </summary>
        /// <returns></returns>
        [HttpGet("Text")]
        public IActionResult Text() 
        {
            return JsonSuccess<string>();
            return JsonData("测试内容");
        }
    }
}