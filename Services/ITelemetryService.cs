using System;
using Overwatcher.Model;
using Overwatcher.Telemetry;

namespace Overwatcher.Services
{
    public interface ITelemetryService
    {
        public void WriteStatus(string sensorId, SensorState state, DateTime time);
        public void WriteTelemetryFrames(string sensorId, TelemetryFrame[] frames, ulong parcelTime);
    }
}