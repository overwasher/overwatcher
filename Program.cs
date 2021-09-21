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
                    // dokku has a convention of giving the INFLUXDB_URL via environment variables
                    // so we read it (along with other env vars) and construct a connection string
                    var influxdbUrl = Environment.GetEnvironmentVariable("INFLUXDB_URL");
                    var influxdbOrganization = Environment.GetEnvironmentVariable("INFLUXDB_ORGANIZATION");
                    var influxdbBucket = Environment.GetEnvironmentVariable("INFLUXDB_BUCKET");
                    var influxdbToken = Environment.GetEnvironmentVariable("INFLUXDB_TOKEN");
                    if (influxdbUrl == null) return;
                    var builder = new UriBuilder(influxdbUrl);
                    
                    var queryString = System.Web.HttpUtility.ParseQueryString(builder.Query);

                    if (influxdbOrganization != null)
                        queryString.Add("org", influxdbOrganization);
                    if (influxdbBucket != null)
                        queryString.Add("bucket", influxdbBucket);
                    if (influxdbToken != null)
                        queryString.Add("token", influxdbToken);
                    
                    builder.Query = queryString.ToString();

                    var memorySource = new MemoryConfigurationSource
                    {
                        InitialData = new[]
                        {
                            new KeyValuePair<string, string>("ConnectionStrings:SensorInfluxDB", builder.ToString())
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
