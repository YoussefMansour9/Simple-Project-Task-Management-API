using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Features.Auth.Commands;
using TaskManagement.Application.Features.Auth.Queries;
using TaskManagement.Application.Features.Project.Commands;
using TaskManagement.Application.Features.Project.Queries;
using TaskManagement.Application.Features.Task.Commands;
using TaskManagement.Application.Features.Task.Queries;

namespace TaskManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProjectMappingProfile), typeof(TaskMappingProfile));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}