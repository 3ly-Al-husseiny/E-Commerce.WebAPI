using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services_Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository,
        ICachRepository cachRepository,
        IAuthService authService,
        UserManager<AppUser> userManager,
        //IConfiguration configuration
        IOptions<JwtOptions> options
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);
        public IBasketService BasketService { get; } = new BasketService(basketRepository,mapper);
        public ICachService CachService { get; } = new CachService(cachRepository);
        public IAuthService AuthService { get; } = new AuthService(userManager,options);
        public IOrderService OrderService { get; } = new OrderService(mapper, basketRepository, unitOfWork);
    }
}
