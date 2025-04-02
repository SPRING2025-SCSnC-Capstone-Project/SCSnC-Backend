using System.Security.Cryptography;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Infrastructure.Data;
using Infrastructure.Services.Deepseek;
using Infrastructure.Services.Identity;
using Infrastructure.Services.Jwt;
using Infrastructure.Services.Security;
using Infrastructure.Services.VNPay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplicationDbContext(configuration)
            .AddJwtAuthentication(configuration)
            .AddVNPayService(configuration)
            .AddDeepseekService(configuration)
            .AddScoped<IApplicationDbContext>(sp => sp.GetService<ApplicationDbContext>()!)
            .AddTransient<ISecurityService, SecurityService>()
            .AddScoped<IIdentityService, IdentityService>()
            .AddScoped<IJwtSService, JwtService>();

        return services;
    }

    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("SCSnC_DB");
        Guard.Against.Null(connectionString, message: "Connection string \"SCSnC_DB\" not found");

        services.AddDbContext<ApplicationDbContext>((sp, builder) =>
        {
            builder.UseNpgsql(connectionString, option =>
            {
                option.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                option.UseNodaTime();
            });
            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configurations) {
       services.Configure<JwtSettings>(configurations.GetSection(JwtSettings.Section));
       services.AddScoped<IJwtSService, JwtService>();

       var signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);

       services.AddSingleton(signingKey);

       services
           .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
           .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer();

       return services;
    }
    
    private static IServiceCollection AddVNPayService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VNPayConfig>(configuration.GetSection(VNPayConfig.Section));
        services.AddTransient<IPaymentService, VNPayService>();

        return services;
    }
    
    private static IServiceCollection AddDeepseekService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DeepSeekConfig>(configuration.GetSection(DeepSeekConfig.Section));
        services.AddTransient<IDeepSeekService, DeepSeekService>();

        services.AddOptions<DeepSeekConfig>()
            .Bind(configuration.GetSection(DeepSeekConfig.Section))
            .Validate(config => !string.IsNullOrEmpty(config.ApiKey))
            .ValidateOnStart();
        
        return services;
    }
}
