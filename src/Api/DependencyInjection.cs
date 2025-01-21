using System.Reflection;
using Api.Policies;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

namespace Api;

public static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(opt =>
        {
            opt.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });

        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // options.SwaggerDoc("v1", new OpenApiInfo()
            // {
            //     Version = "v1",
            //     Title = "SCSnC API",
            //     Description = "API for SCSnC application",
            // });
            //
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    } 
}