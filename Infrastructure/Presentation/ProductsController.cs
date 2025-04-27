using Microsoft.AspNetCore.Mvc;
using Services_Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // endpoint : public non-static method
        [HttpGet] // endpoint : GET:api/Products
        public async Task<IActionResult> GetAllProducts() 
        {
            var result = await serviceManager.ProductService.GetAllProductAsync();
            if (result is null)
                return BadRequest(); // 400 
            return Ok(result); // Status Code 200 
        }




    }
}
