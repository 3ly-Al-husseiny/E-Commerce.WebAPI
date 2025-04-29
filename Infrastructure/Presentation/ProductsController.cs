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
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {

        // sort : name(Asc) [Default]
        // sort : name(Dsc)
        // sort : price(Asc)
        // sort : price(Dsc)



        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null)
                return BadRequest(); //400
            return Ok(result);
        }



        // endpoint : public non-static method
        [HttpGet("products")] // endpoint : GET:api/Products
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductSpecificationParameters SpecParams)
        {
            var result = await serviceManager.ProductService.GetAllProductAsync(SpecParams);
            if (result is null)
                return BadRequest(); // 400 
            return Ok(result); // Status Code 200 
        }


        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes() 
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null)
                return BadRequest();
            return Ok(result);
        }


        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands() 
        {
            var result = await serviceManager.ProductService.GetAllBrands();
            if (result is null)
                return BadRequest();
            return Ok(result);
        }
    }
}
