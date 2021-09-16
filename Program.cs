using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;

namespace Overwatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(o =>
                {
                    var influxdbUrl = Environment.GetEnvironmentVariable("INFLUXDB_URL");
                    if (influxdbUrl == null) return;
                    var memorySource = new MemoryConfigurationSource
                    {
                        InitialData = new[]
                        {
                            new KeyValuePair<string, string>("ConnectionStrings:SensorInfluxDB", influxdbUrl)
                        }
                    };
                    o.Add(memorySource);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
