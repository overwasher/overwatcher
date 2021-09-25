using System;
using System.Linq;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Overwatcher.Database;
using Overwatcher.Model;
using Overwatcher.Telemetry;

namespace Overwatcher.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly TelemetryDbContext _db;

        public TelemetryService(TelemetryDbContext db)
        {
            _db = db;
        }

        public void WriteStatus(string sensorId, SensorState state, DateTime time)
        {
            _db.WriteApi.WritePoint(
                    PointData.Measurement("sensor_status")
                        .Timestamp(time, WritePrecision.Ms)
                        .Tag("sensor_id", sensorId)
                        .Field("status", state == SensorState.Active ? 1 : 0)
                );
        }

        public void WriteTelemetryFrames(string sensorId, TelemetryFrame[] frames, ulong parcelTime)
        {
            _db.WriteApi.WritePoints(frames.Select(f =>
                PointData.Measurement("acceleration")
                    .Timestamp(f.Time, WritePrecision.Ms)
                    .Tag("sensor_id", sensorId)
                    .Field("parcel_time", parcelTime)
                    .Field("x", f.X)
                    .Field("y", f.Y)
                    .Field("z", f.Z)
            ).ToArray());
        }
    }
}