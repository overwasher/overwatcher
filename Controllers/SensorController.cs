using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overwatcher.Model;
using Overwatcher.Services;

namespace Overwatcher.Controllers
{
    [ApiController]
    [Route("/sensor")]
    [Authorize]
    public class SensorController : ControllerBase
    {
        private readonly ISensorStatusWriter _statusWriter;

        public SensorController(ISensorStatusWriter statusWriter)
        {
            _statusWriter = statusWriter;
        }

        [HttpPost]
        [Route("v1/update")]
        public bool PostV1Update([FromBody] SensorNodeUpdate update)
        {
            var sensorId = HttpContext.GetSensorId();
            
            _statusWriter.Update(sensorId, update.State);
            
            return true;
        }
    }
}