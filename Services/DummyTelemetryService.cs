using System;
using Overwatcher.Model;

namespace Overwatcher.Services
{
    public class DummyTelemetryService : ITelemetryService
    {
        public void WriteStatus(string sensorId, SensorState state, DateTime time)
        {
            // ignore
        }
    }
}