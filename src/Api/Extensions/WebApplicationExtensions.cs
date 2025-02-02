using Api.Middlewares;
using Infrastructure.Data;

namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, IConfiguration configuration)
    {
        // Configure the HTTP request pipeline.
        
        // Handle all exceptions: return appropriate status codes and messages
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
            // app.UseCors(CORSPolicy.Development);
            
            app.MigrateDatabase<ApplicationDbContext>();
        }

        // if (app.Environment.IsEnvironment("Production"))
        // {
        //     app.UseCors(CORSPolicy.Production);
        //     app.MigrateDatabase<ApplicationDbContext>((_, _) =>
        //     {
        //         // TODO: should only generate admin account 
        //     });
        // }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => "Hello from SCSnC!");
    }
}