using System;

namespace Overwatcher.Model
{
    public record DumpedSensorState
    {
        public string Id { get; init; }
        public SensorState State { get; init; }
        public DateTime LastContact { get; init; }
    }
}