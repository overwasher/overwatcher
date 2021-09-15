using Overwatcher.Model;

namespace Overwatcher.Services
{
    public interface ISensorStatusWriter
    {
        public void Update(string sensorId, SensorState newState);
    }
}