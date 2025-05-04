using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstraction
{
    public interface IOrderService
    {
        // Get Order By Id 
        Task<OrderResultDto> GetOrderByIdAsync(Guid id);

        // Get All Orders By UserEmail 
        Task<IEnumerable<OrderResultDto>> GetAllOrdersByUserEmailAsync(string email);

        // Create Order 
        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest , string userEmail);

        // Get All Delivery Methods 
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();
    }
}
