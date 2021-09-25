using System;
using Overwatcher.Model;
using Overwatcher.Telemetry;

namespace Overwatcher.Services
{
    public class DummyTelemetryService : ITelemetryService
    {
        public void WriteStatus(string sensorId, SensorState state, DateTime time)
        {
            // ignore
        }

        public void WriteTelemetryFrames(string sensorId, TelemetryFrame[] frames, ulong parcelTime)
        {
            // ignore
        }
    }
}