using System.Reflection;
using Architecture.Api.Abstractions;
using Architecture.Api.Common;
using Architecture.Api.Infrastructure.Database;
using Architecture.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Architecture.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging());
            services.Scan(scan => scan
                .FromAssemblyOf<UserRepository>()
                .AddClasses(classes => classes.InNamespaces("Architecture.Api.Infrastructure.Repositories")
                    .Where(type => type != typeof(Command)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
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