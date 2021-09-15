using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Overwatcher
{
    public static class Extensions
    {
        public static string GetSensorId(this HttpContext context)
        {
            return context.User.Claims.Single(c => c.Type == "sensorId").Value;
        }
    }
}