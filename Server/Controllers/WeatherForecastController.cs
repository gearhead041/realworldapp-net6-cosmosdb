using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.NET6.CosmosDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public WeatherForecastController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }
    }
}