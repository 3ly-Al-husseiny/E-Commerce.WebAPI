
using Domain.Contracts;
using E_Commerce.WebAPI.Extensions;
using E_Commerce.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using Presentation;
using Services;
using Services_Abstraction;
using Shared.ErroModels;
using System.Threading.Tasks;
namespace E_Commerce.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();
            app.Run();

        }
    }
}
