using Architecture.Api.Infrastructure;
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
                .UseLazyLoadingProxies());
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

            return services;
        }
    }
}