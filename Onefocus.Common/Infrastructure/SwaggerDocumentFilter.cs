using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Onefocus.Common.Infrastructure
{
    public class SwaggerDocumentFilter(KeyValuePair<string, string>[] basePaths) : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in basePaths)
            {
                swaggerDoc.Servers.Add(new OpenApiServer() { Description = path.Key, Url = path.Value });
            }
        }
    }
}
