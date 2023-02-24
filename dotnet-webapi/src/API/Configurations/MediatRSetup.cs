using Application.Common;
using Application.Common.Behaviors;
using Application.Common.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Api.Configurations;

public static class MediatRSetup
{
    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Application.IAssemblyMarker).GetTypeInfo().Assembly);

        services.AddScoped<INotificationHandler<ValidationError>, ValidationErrorHandler>();


        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}