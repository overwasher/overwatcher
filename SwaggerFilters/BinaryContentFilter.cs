using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Overwatcher.SwaggerFilters
{
    public class BinaryContentFilter : IOperationFilter
    {
        /// <summary>
        /// Configures operations decorated with the <see cref="BinaryContentAttribute" />.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attribute = context.MethodInfo.GetCustomAttributes(typeof(BinaryContentAttribute), false).FirstOrDefault();
            if (attribute == null)
            {
                return;
            }

            operation.RequestBody = new OpenApiRequestBody() { Required = true };
            operation.RequestBody.Content.Add("application/octet-stream", new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary",
                },
            });
        }
    }

}