using Microsoft.AspNetCore.Mvc;
using Services_Abstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasetById(string id)
        {
            var result = await serviceManager.BasketService.GetBasketAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket([FromBody] BasketDto basket)
        {
            var result = await serviceManager.BasketService.UpdateBasketAsync(basket);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var result = await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
