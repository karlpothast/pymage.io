using Application.Common;
using Application.MappingProfiles;
using Infrastructure.Context;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        services.AddScoped<IContext, ApplicationDbContext>();
        NewId.SetProcessIdProvider(new CurrentProcessIdProvider());

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}