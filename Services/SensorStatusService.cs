using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Overwatcher.Model;

namespace Overwatcher.Services
{
    public class SensorStatusService : ISensorStatusWriter, ISensorStatusReader
    {
        private readonly Dictionary<string, (SensorState, DateTime)> _store = new();
        private readonly DateTimeProvider _dateTimeProvider;
        private readonly object _lock = new object();
        private readonly TimeSpan _staleInterval;
        private readonly ITelemetryService _telemetryService;
        
        public SensorStatusService(DateTimeProvider dateTimeProvider, IConfiguration configuration, ITelemetryService telemetryService)
        {
            _dateTimeProvider = dateTimeProvider;
            _telemetryService = telemetryService;
            _staleInterval = TimeSpan.FromSeconds(configuration.GetValue<double>("StaleInterval"));
        }

        public void Update(string sensorId, SensorState newState)
        {
            var time = _dateTimeProvider.UtcNow;
            lock (_lock)
            {
                _store[sensorId] = (newState, time);
            }
            _telemetryService.WriteStatus(sensorId, newState, time);
        }

        private DumpedSensorState DumpSensorState(KeyValuePair<string, (SensorState, DateTime)> pair, DateTime time)
        {
            var stale = pair.Value.Item2 + _staleInterval < time;

            return new DumpedSensorState
            {
                Id = pair.Key,
                State = stale ? SensorState.Unknown : pair.Value.Item1,
                LastContact = pair.Value.Item2,
            };
        }
        
        public DumpedSensorState[] GetStates()
        {
            lock (_lock)
            {
                var time = _dateTimeProvider.UtcNow;

                return _store
                    .Select(pair => DumpSensorState(pair, time))
                    .ToArray();
            }
        }
    }
}