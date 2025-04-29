using Microsoft.AspNetCore.Mvc;
using Presentation;
using Shared.ErroModels;
using Services;
using System.Runtime.CompilerServices;
using Domain.Contracts;
using E_Commerce.WebAPI.Middlewares;
using Presentation;
namespace E_Commerce.WebAPI.Extensions
{
    public static class Extesions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.ConfigureServices();
            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();

            return services;  
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                     .Select(m => new ValidationError()
                     {
                         Field = m.Key,
                         Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                     });
                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }

        public static async Task<WebApplication> ConfigureMiddlewares (this WebApplication app) 
        {
            await app.InitializeDataBaseAsync();
            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }




            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            

            return app;
        }

        public static async Task<WebApplication> InitializeDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync(); // Call The Function
            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();
            return app;
        }

    }
}
