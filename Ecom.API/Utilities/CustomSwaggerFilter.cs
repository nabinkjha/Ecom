using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ECom.API.Utilities
{
    public class CustomSwaggerFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var excludeRoutes = swaggerDoc.Paths
                .Where(x => !x.Key.ToLower().Contains(context.DocumentName))
                .ToList();
            excludeRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }
    }
    public class CustomHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-key",
                In = ParameterLocation.Header,
                Description = "12345abecdf516d0f6472d5199999",
                Required = true // set to false if this is optional
            });
        }
    }
}
