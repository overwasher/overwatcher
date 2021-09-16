using System;
using Overwatcher.Model;

namespace Overwatcher.Services
{
    public interface ITelemetryService
    {
        public void WriteStatus(string sensorId, SensorState state, DateTime time);
    }
}