using System;

namespace Overwatcher.Services
{
    public interface ITelemetryParcelProcessor
    {
        void ProcessParcel(string sensorId, Memory<byte> parcelBody);
    }
}