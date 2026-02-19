using System.Net;
using System.Net.Mail;
using System.Reflection;
using Architecture.Application.Abstractions;
using Architecture.Application.Abstractions.Services;
using Architecture.Application.Attributes;
using Architecture.Infrastructure.Database;
using Architecture.Infrastructure.Database.Repositories;
using Architecture.Infrastructure.Services;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Architecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging());
            services.Scan(scan => scan
                .FromAssemblyOf<UserRepository>()
                .AddClasses(classes => classes.InNamespaces("Architecture.Infrastructure")
                    .Where(type => type != typeof(Command)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.AddTransient<SmtpClient>((serviceProvider) =>
            {
                return new SmtpClient
                {
                    Host = configuration["AppSettings:Mail:Smtp"],
                    Port = int.Parse(configuration["AppSettings:Mail:Port"]),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(configuration["AppSettings:Mail:User"], configuration["AppSettings:Mail:Password"])
                };
            });
            services.AddTransient<IEmailService, EmailService>();
            services.AddHttpClient<ISMSService, SMSService>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(configuration["AppSettings:SMS:Server"]);
                client.DefaultRequestHeaders.Add("Authorization", configuration["AppSettings:SMS:Secret"]);
                client.Timeout = new TimeSpan(0, 1, 0);
            });

            return services;
        }

        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IJob>()
                .AddClasses(x => x.AssignableTo<IJob>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddHangfire(options =>
            {
                options.UseSimpleAssemblyNameTypeSerializer();
                options.UseRecommendedSerializerSettings();
                options.UseInMemoryStorage();
            });

            services.AddHangfireServer();

            return services;
        }

        public static IApplicationBuilder UseJobs(this IApplicationBuilder app)
        {
            var manager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

            Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(IJob).IsAssignableFrom(x))
                .ToList()
                .ForEach(type =>
                {
                    var schedule = type.GetCustomAttribute<JobScheduleAttribute>();

                    if (schedule != null)
                    {
                        var job = new Job(type, type.GetMethod(nameof(IJob.RunAsync)));

                        manager.AddOrUpdate(type.FullName, job, schedule.Cron);
                    }
                });

            return app;
        }

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();

            return app;
        }
    }
}