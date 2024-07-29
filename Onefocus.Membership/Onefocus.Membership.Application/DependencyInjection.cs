﻿using Microsoft.Extensions.DependencyInjection;
using Onefocus.Membership.Infrastructure.Databases.Repositories;

namespace Onefocus.Membership.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
