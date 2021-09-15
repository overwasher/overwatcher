using Overwatcher.Model;

namespace Overwatcher.Services
{
    public interface ITelemetryService
    {
        public void WriteStatus(string sensorId, SensorNodeUpdate state);
    }
}