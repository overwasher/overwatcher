using System.Runtime.InteropServices;

namespace Overwatcher.Telemetry
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct TelemetryParcelRawFrame
    {
        public short X;
        public short Y;
        public short Z;
    }
}