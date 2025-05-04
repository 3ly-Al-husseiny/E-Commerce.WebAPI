using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Data.Identity;
using Persistence.Repositories;
using Services;
using Services_Abstraction;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public static class InfrastructureServiceRegisteration
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<E_CommerceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDbContext<E_CommerceIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IDentityConnection"));
            });
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IConnectionMultiplexer>(ServiceProvider => 
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            });
            services.AddScoped<ICachRepository, CachRepository>();
            return services;
        }   


    }
}
