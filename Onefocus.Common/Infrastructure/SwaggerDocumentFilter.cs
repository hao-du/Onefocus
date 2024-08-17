using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Infrastructure
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        private readonly KeyValuePair<string, string>[] _basePaths;

        public SwaggerDocumentFilter(KeyValuePair<string, string>[] basePaths)
        {
            _basePaths = basePaths;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach(var path in _basePaths)
            {
                swaggerDoc.Servers.Add(new OpenApiServer() { Description = path.Key, Url = path.Value });
            }
        }
    }
}
