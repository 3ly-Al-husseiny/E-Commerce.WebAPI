using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services_Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ControllerBase
    {

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderRequestDto request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.CreateOrderAsync(request, email);
            return Ok(result);
        }


        // Get All Orders for the Current Loged in User. 
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetAllOrdersByUserEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var result = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("Delivery")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await serviceManager.OrderService.GetAllDeliveryMethods();
            return Ok(result);
        }



    }
}
