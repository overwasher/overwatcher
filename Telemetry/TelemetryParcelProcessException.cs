using System;

namespace Overwatcher.Telemetry
{
    public class TelemetryParcelProcessException : Exception
    {
        public TelemetryParcelProcessException(string message)
            : base(message)
        {
        }
    }
}