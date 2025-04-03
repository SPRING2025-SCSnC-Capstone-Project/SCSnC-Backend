using Api;
using Api.Extensions;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Configuration.AddAppConfiguration();
    builder.Services
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration)
        .AddApiServices(builder.Configuration);
    var app = builder.Build();
    
    app.UseCors(builder =>
        builder
            .SetIsOriginAllowed(_ => true));
    
    app.UseInfrastructure(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    // Handle an error related to .NET 6
    // https://github.com/dotnet/runtime/issues/60600
    var error = ex.GetType().Name;
    if (error.Equals("HostAbortedException", StringComparison.Ordinal))
    {
        throw;
    }
}
