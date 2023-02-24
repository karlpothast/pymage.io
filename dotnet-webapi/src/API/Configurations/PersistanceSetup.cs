using Application.Auth;
using Domain.Auth.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configurations;

public static class PersistanceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ISession, Session>();
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}