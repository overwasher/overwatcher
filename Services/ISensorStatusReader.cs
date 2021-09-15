using Overwatcher.Model;

namespace Overwatcher.Services
{
    public interface ISensorStatusReader
    {
        public DumpedSensorState[] GetStates();
    }
}