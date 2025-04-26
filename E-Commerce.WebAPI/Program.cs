
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;
using Persistence.Data.Contexts;
using System.Threading.Tasks;

namespace E_Commerce.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            
            builder.Services.AddControllers();


            //Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // E_CommereceDbContext Dependency Injection Service
            builder.Services.AddDbContext<E_CommerceDbContext>(options =>

            // GetConnectionString("ConnectionString Key") Extension Method to Get the Connection String 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // The Normal Way to Get the Data From the AppSettings
            //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);


            // Allow DI For DbInitializer
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            #endregion


            var app = builder.Build();


            #region DbInitializer - Seeding
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync(); // Call The Function

            #endregion

            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
