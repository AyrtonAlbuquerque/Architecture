using System.Reflection;
using Architecture.Application.Abstractions;
using Architecture.Application.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Architecture.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddOptions<Settings>()
                .BindConfiguration(Settings.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton(x => x.GetRequiredService<IOptions<Settings>>().Value);
            services.AddSingleton<IToken, Token>();

            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(IMapping).IsAssignableFrom(x))
                .Select(x => (IMapping)Activator.CreateInstance(x)!).ToList()
                .ForEach(x => x?.AddMapping());

            return services;
        }
    }
}