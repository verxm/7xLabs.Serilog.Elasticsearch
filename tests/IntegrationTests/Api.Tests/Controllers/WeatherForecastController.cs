using Microsoft.AspNetCore.Mvc;

namespace Api.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromHeader] int tenantId)
        {
            using var logScope = _logger.BeginScope(new Dictionary<string, object>
                {
                    {"Key1", "Value" },
                    {"Key2", 10 },
                    {"Key3", new {prop0 = 29, prop = "teste"} },
                });

            try
            {
                _logger.LogInformation("Any log test {@anyObj}", new { test = "Test", test2 = 27 });

                throw new AnyException(new("Value 1", 10), 20);

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }

    public class AnyException : Exception
    {
        public int MyProperty { get; set; }
        public AnyExceptionData AnyExceptionData { get; set; }

        public AnyException(AnyExceptionData data, int myProperty) : base("Any exception error message")
        {
            AnyExceptionData = data;
            MyProperty = myProperty;
        }
    }

    public record AnyExceptionData(string Prop1, int Prop2);
}
