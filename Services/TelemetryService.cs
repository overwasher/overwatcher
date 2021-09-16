using System;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Overwatcher.Database;
using Overwatcher.Model;

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
    }
}