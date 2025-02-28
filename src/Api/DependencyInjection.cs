using System.Reflection;
using Api.Middlewares;
using Api.Policies;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddControllers(opt =>
        {
            opt.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });

        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "SCSnC API",
                Description = "API for SCSnC application",
            });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme() {
                        Reference = new OpenApiReference() {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    new string[]{}
                },
            });
        });

        return services;
    } 
}
