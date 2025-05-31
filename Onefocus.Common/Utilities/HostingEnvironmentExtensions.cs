using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Utilities;

public static class HostingEnvironmentExtensions
{
    public static bool IsDevelopmentLike(this IHostEnvironment env)
    {
        return !env.IsProduction() && !env.IsStaging();
    }
}