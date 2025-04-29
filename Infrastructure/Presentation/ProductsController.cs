using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services_Abstraction;
using Shared;
using Shared.ErroModels;
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
        [HttpGet()] // endpoint : GET:api/Products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagicationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Cach(60)]
        public async Task<ActionResult<PagicationResponse<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecificationParameters SpecParams)
        {
            var result = await serviceManager.ProductService.GetAllProductAsync(SpecParams);
            if (result is null)
                return BadRequest(); // 400 
            return Ok(result); // Status Code 200 
        }

        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]

        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrands();
            if (result is null)
                return BadRequest();
            return Ok(result);
        }
    }
}
