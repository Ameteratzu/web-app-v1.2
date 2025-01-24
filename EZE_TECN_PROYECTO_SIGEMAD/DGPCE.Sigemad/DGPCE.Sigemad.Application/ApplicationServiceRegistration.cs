using DGPCE.Sigemad.Application.Behaviours;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Queries;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DGPCE.Sigemad.Application;

public static class ApplicationServiceRegistration
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        // Registrar GetRegistrosPorSucesoQueryHandler
        services.AddTransient<GetRegistrosPorSucesoQueryHandler>();




        return services;
    }

}
