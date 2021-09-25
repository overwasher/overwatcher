using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Overwatcher.Telemetry;

namespace Overwatcher.Services
{
    public class TelemetryParcelProcessor : ITelemetryParcelProcessor
    {
        private const int BufferAlignment = 1024;

        private readonly ITelemetryService _telemetryService;

        public TelemetryParcelProcessor(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        void Assert(bool value, string message = "Telemetry Parcel Assertion Failed")
        {
            if (!value)
                throw new TelemetryParcelProcessException(message);
        }

        private unsafe TelemetryFrame[] ReadFrames(ReadOnlySpan<byte> dataSpan, long finishTimeUs, double rate)
        {
            var frameSize = sizeof(TelemetryParcelRawFrame);
            var framesInBuffer = BufferAlignment / frameSize;
            
            var bufferCount = dataSpan.Length / BufferAlignment;
            var frameCount = bufferCount * framesInBuffer;
            var res = new TelemetryFrame[frameCount];

            var usPerFrame = 1000000 / rate;
            var startTimeUs = finishTimeUs - usPerFrame * frameCount;
            
            for (var bufferIdx = 0; bufferIdx < bufferCount; bufferIdx++)
            {
                var frames = MemoryMarshal.Cast<byte, TelemetryParcelRawFrame>(dataSpan
                        .Slice(bufferIdx * BufferAlignment, framesInBuffer * frameSize)
                    );
                
                for (var frameIdx = 0; frameIdx < frames.Length; frameIdx++)
                {
                    var rawFrame = frames[frameIdx];
                    
                    var globalFrameIdx = bufferIdx * framesInBuffer + frameIdx;
                    var time = (long)(startTimeUs + globalFrameIdx * usPerFrame);

                    var frame = new TelemetryFrame
                    {
                        Time = DateTime.UnixEpoch.AddTicks(time * (TimeSpan.TicksPerMillisecond / 1000)),
                        X = rawFrame.X,
                        Y = rawFrame.Y,
                        Z = rawFrame.Z
                    };

                    res[globalFrameIdx] = frame;
                }
            }

            return res;
        }
        
        public unsafe void ProcessParcel(string sensorId, Memory<byte> parcelBody)
        {
            var parcelSpan = parcelBody.Span;
            var genericHeader = MemoryMarshal.Read<TelemetryParcelHeaderGeneric>(parcelSpan);
            
            Assert(genericHeader.Magic == 0x4c54574f, "Magic mismatch");
            Assert(genericHeader.Version == 1, "Unsupported version");

            var header = MemoryMarshal.Read<TelemetryParcelHeaderV1>(parcelSpan);

            var dataSpan = parcelSpan.Slice(sizeof(TelemetryParcelHeaderV1));

            Assert(dataSpan.Length % 1024 == 0, "Invalid data alignment");

            var frames = ReadFrames(dataSpan, checked((long) header.RealTimestamp),
                (double) header.UpdateRateNominator / header.UpdateRateDenominator);
            
            _telemetryService.WriteTelemetryFrames(sensorId, frames, header.RealTimestamp);
        }
    }
}