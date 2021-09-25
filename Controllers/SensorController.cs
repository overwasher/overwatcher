using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using Overwatcher.Model;
using Overwatcher.Services;
using Overwatcher.SwaggerFilters;

namespace Overwatcher.Controllers
{
    [ApiController]
    [Route("/sensor")]
    [Authorize]
    public class SensorController : ControllerBase
    {
        private readonly ISensorStatusWriter _statusWriter;
        private readonly ITelemetryParcelProcessor _telemetryProcessor;

        public SensorController(ISensorStatusWriter statusWriter, ITelemetryParcelProcessor telemetryProcessor)
        {
            _statusWriter = statusWriter;
            _telemetryProcessor = telemetryProcessor;
        }

        /// <summary>
        /// Send a state update from the sensor node
        /// </summary>
        /// <remarks>
        /// Requires a valid sensor node bearer token in Authorization field
        /// </remarks>
        [HttpPost]
        [Route("v1/update")]
        public bool PostV1Update([FromBody] SensorNodeUpdate update)
        {
            var sensorId = HttpContext.GetSensorId();
            
            _statusWriter.Update(sensorId, update.State);
            
            return true;
        }

        /// <summary>
        /// Send a telemetry parcel from the sensor node
        /// </summary>
        /// <remarks>
        /// Requires a valid sensor node bearer token in Authorization field
        /// 
        /// Accepts a binary telemetry parcel as a body
        /// TODO: document the binary telemetry parcel format
        /// </remarks>
        [HttpPost]
        [Route("v1/telemetry")]
        [BinaryContent]
        public async Task<IActionResult> PostV1Telemetry()
        {
            var maxPayloadSize = 2 * 1024 * 1024; // 2 MiB
            var ms = new MemoryStream();
            var buffer = new byte[4096];

            while (true)
            {
                var actualRead = await Request.Body.ReadAsync(buffer);
                if (actualRead == 0)
                    break;
                ms.Write(buffer.AsSpan().Slice(0, actualRead));
                if (ms.Length > maxPayloadSize)
                    return StatusCode(413); // Payload Too Large
            }
            
            var telemetryData = ms.GetBuffer().AsMemory().Slice(0, (int)ms.Length);
            
            _telemetryProcessor.ProcessParcel(HttpContext.GetSensorId(), telemetryData);

            return Ok(true);
        }
    }
}