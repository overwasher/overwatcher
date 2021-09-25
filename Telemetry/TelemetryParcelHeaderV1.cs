using System.Runtime.InteropServices;

namespace Overwatcher.Telemetry
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public unsafe struct TelemetryParcelHeaderV1
    {
        public uint Magic;
        public uint Version;
        public uint LocalTimestamp;
        public ulong RealTimestamp;
        public uint UpdateRateNominator;
        public uint UpdateRateDenominator;
        public fixed byte AppVersion[32];
        public fixed byte CompileTime[16];
        public fixed byte CompileDate[16];
        public fixed byte IdfVersion[32];
        public fixed byte AppElfSha256[32];
    }
}