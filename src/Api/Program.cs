using Api;
using Api.Extensions;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);
    var app = builder.Build();

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