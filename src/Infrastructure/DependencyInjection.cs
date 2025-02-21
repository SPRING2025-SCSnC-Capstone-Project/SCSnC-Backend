using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Infrastructure.Data;
using Infrastructure.Services.VNPay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDbContext(configuration);
        services.AddScoped<IApplicationDbContext>(sp => sp.GetService<ApplicationDbContext>()!);
        services.AddScoped<IPaymentService>(p => p.GetService<VNPayService>()!);
    }

    private static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
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
        });
    }
}