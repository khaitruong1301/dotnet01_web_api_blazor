using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRepoPatternController : ControllerBase
    {
        IProductService _productService;
        public ProductRepoPatternController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productService.GetAllProduct());
        }
    }
}


