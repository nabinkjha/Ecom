using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
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
}
