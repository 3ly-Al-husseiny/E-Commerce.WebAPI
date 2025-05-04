using Microsoft.AspNetCore.Mvc;
using Presentation;
using Shared.ErroModels;
using Services;
using System.Runtime.CompilerServices;
using Domain.Contracts;
using E_Commerce.WebAPI.Middlewares;
using Presentation;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data.Contexts;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Persistence.Data.Identity;
namespace E_Commerce.WebAPI.Extensions
{
    public static class Extesions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.AddIdentityServices(); 
            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices(configuration);
            services.ConfigureServices();
            services.ConfigureJwtOptions(configuration);
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


        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app)
        {
            await app.InitializeDataBaseAsync();
            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();

            if (app.Environment.IsDevelopment())
            {
                app.UseRouting();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.UseGlobalErrorHandling();

            return app;
        }

        public static async Task<WebApplication> InitializeDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();
            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();
            return app;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
           .AddEntityFrameworkStores<E_CommerceIdentityDbContext>();
            return services;

        }

        private static IServiceCollection ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration) 
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

                ValidateLifetime = true,

            });

            return services;
        }
    }
}
