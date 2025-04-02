using System.Text;
using Architecture.Api.Extensions;
using Architecture.Api.Handlers;
using Architecture.Application;
using Architecture.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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


            builder.Services.AddOpenApi();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRouting();
            builder.Services.AddAuthorization();
            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddEndpoints();
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            // Add Logging
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            // Add Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Architecture API",
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
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.CustomSchemaIds(type => type.ToString());
            });

            // Add Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.AllowAnyOrigin();
                });
            });

            // Add Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler();
            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.MapOpenApi();
            app.MapEndpoints();
            app.Run();
        }
    }
}