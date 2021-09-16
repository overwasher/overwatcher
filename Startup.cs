using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Overwatcher.Auth;
using Overwatcher.Converters;
using Overwatcher.Services;

namespace Overwatcher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                // deprecated, but still required
                // See https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1269
                // System.Text.Json is used, but swagger can't detect the enum handling for it
#pragma warning disable 618
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
#pragma warning restore 618
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "overwatcher", Version = "v1" });
                c.MapType<DateTime>(() => new OpenApiSchema
                {
                    Type = "integer",
                    Example = new OpenApiLong(1631740551905),
                    Description = "Unix timestamp millis"
                });
            });
            
            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
                    o.JsonSerializerOptions.Converters.Add(enumConverter);

                    o.JsonSerializerOptions.Converters.Add(new DateTimeConverterUnixMillis());
                });
 
            if (Configuration.GetValue<bool>("Jwt:Enable")) {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(o =>
                    {
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = false,
                            IssuerSigningKey = new JsonWebKey(Configuration["Jwt:Key"]),
                        };
                        o.SaveToken = false;
                        o.MapInboundClaims = false;
                    });
            }
            else
            {
                services.AddAuthentication(DummySensorAuthHandler.AuthenticationScheme)
                    .AddScheme<DummySensorAuthOptions, DummySensorAuthHandler>(DummySensorAuthHandler.AuthenticationScheme, o =>
                    {
                    });
            }

            services.AddTransient<DateTimeProvider>();
            
            services.AddSingleton<SensorStatusService>();
            services.AddSingleton<ISensorStatusReader>(x => x.GetRequiredService<SensorStatusService>());
            services.AddSingleton<ISensorStatusWriter>(x => x.GetRequiredService<SensorStatusService>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("InfluxDB URL is {0}", Configuration.GetValue<string>("SensorInfluxDB:Url"));
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "overwatcher v1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
