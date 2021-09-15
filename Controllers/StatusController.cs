using Microsoft.AspNetCore.Mvc;
using Overwatcher.Model;
using Overwatcher.Services;

namespace Overwatcher.Controllers
{
    [ApiController]
    [Route("/status")]
    public class StatusController : ControllerBase
    {
        private readonly ISensorStatusReader _sensorStatusReader;

        public StatusController(ISensorStatusReader sensorStatusReader)
        {
            _sensorStatusReader = sensorStatusReader;
        }

        [HttpGet]
        [Route("v1")]
        public DumpedSensorState[] GetSensorStates()
        {
            return _sensorStatusReader.GetStates();
        }
    }
}