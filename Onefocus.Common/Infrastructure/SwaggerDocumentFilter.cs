using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Infrastructure
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        private readonly string _basePath;

        public SwaggerDocumentFilter(string basePath)
        {
            _basePath = basePath;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Servers.Add(new OpenApiServer() { Url = _basePath });
        }
    }
}
