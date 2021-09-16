using System;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;

namespace Overwatcher.Database
{
    public record TelemetryDbContext : IDisposable
    {
        private readonly ILogger<TelemetryDbContext> _logger;

        public TelemetryDbContext(string connectionString, ILogger<TelemetryDbContext> logger)
        {
            _logger = logger;
            InfluxDbClient = InfluxDBClientFactory.Create(connectionString);
            WriteApi = InfluxDbClient.GetWriteApi();
            
            WriteApi.EventHandler += WriteApiOnEventHandler;
        }

        private void WriteApiOnEventHandler(object? sender, EventArgs eventArgs)
        {
            if (eventArgs is WriteErrorEvent errorEvent)
            {
                var exception = errorEvent.Exception;
        
                _logger.LogError("Error occurred when writing to the database: {0}", exception);
            }

            if (eventArgs is WriteRetriableErrorEvent retriableErrorEvent)
            {
                var exception = retriableErrorEvent.Exception;
                
                _logger.LogWarning("Retriable error occurred when writing to the database: {0}", exception);
            }
        }

        public InfluxDBClient InfluxDbClient { get; }
        public WriteApi WriteApi { get; }

        public void Dispose()
        {
            WriteApi?.Dispose();
            InfluxDbClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}