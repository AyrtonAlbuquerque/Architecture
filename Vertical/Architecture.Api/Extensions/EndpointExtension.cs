using System.Reflection;
using Architecture.Api.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Architecture.Api.Extensions
{
    public static class EndpointExtension
    {
        public static IServiceCollection AddEndpoints(this IServiceCollection services)
        {
            services.TryAddEnumerable(Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray());

            return services;
        }

        public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? groupBuilder = null)
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
            IEndpointRouteBuilder builder = groupBuilder is null ? app : groupBuilder;

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder);
            }

            return app;
        }
    }
}