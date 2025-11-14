using System.Text;
using Architecture.Api.Extensions;
using Architecture.Api.Filters;
using Architecture.Api.Handlers;
using Architecture.Application;
using Architecture.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace Architecture.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var secret = builder.Configuration["AppSettings:Jwt:Secret"];
            var issuer = builder.Configuration["AppSettings:Jwt:Issuer"];
            var audience = builder.Configuration["AppSettings:Jwt:Audience"];
            var environment = builder.Configuration["AppSettings:Environment"];

            ArgumentException.ThrowIfNullOrEmpty(secret);
            ArgumentException.ThrowIfNullOrEmpty(issuer);
            ArgumentException.ThrowIfNullOrEmpty(audience);

            builder.Services
                .AddOpenApi()
                .AddMemoryCache()
                .AddHttpContextAccessor()
                .AddEndpointsApiExplorer()
                .AddRouting()
                .AddAuthorization()
                .AddExceptionHandler<ExceptionHandler>()
                .AddProblemDetails()
                .AddEndpoints()
                .AddMappings()
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyMethod();
                        policy.AllowAnyHeader();
                        policy.AllowAnyOrigin();
                    });
                })
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Clean Architecture API",
                        Version = $"1.0 {environment}",
                        Description = "Rest API",
                        Contact = new OpenApiContact
                        {
                            Name = "Ayrton Albuquerque",
                            Email = "ayrton_ito@hotmail.com"
                        }
                    });
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization Bearer Token.",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                    options.CustomSchemaIds(type =>
                    {
                        return string.Join('.', type.FullName?.Split('.').TakeLast(2) ?? new[] { type.Name })
                            .Replace("Command", "Request")
                            .Replace("Query", "Request");
                    });
                    options.OperationFilter<ProblemDetailsFilter>();
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                })
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            var app = builder.Build();

            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
            app.MapEndpoints();
            app.Run();
        }
    }
}