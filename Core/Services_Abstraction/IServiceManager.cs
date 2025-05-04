using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstraction
{
    public interface IServiceManager
    {
       IProductService ProductService { get;}
        IBasketService BasketService { get; }
        ICachService CachService { get; }
        IAuthService AuthService { get; }
        IOrderService OrderService { get; }
    }
}
