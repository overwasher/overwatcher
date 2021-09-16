using System;

namespace Overwatcher.Model
{
    public record DumpedSensorState
    {
        public string Id { get; init; } = null!;
        public SensorState State { get; init; }
        public DateTime LastContact { get; init; }
    }
}