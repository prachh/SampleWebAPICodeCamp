using System;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using SampleWebAPICodeCamp.Controllers;
using Xunit;

namespace SampleWebAPICodeCamp.Test
{
    public class WeatherForecastControllerTest
    {
        private readonly WeatherForecastController _weatherForecastController;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastControllerTest()
        {
            _logger = A.Fake<ILogger<WeatherForecastController>>();
            _weatherForecastController = new WeatherForecastController(_logger);
        }


        [Fact]
        public void Get()
        {
            var result = _weatherForecastController.Get();

            Assert.NotEmpty(result);
        }
    }
}
