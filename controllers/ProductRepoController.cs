using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRepoController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductRepoController(IProductService prodService)
        {
            _productService = prodService;
        }

        [HttpGet("GetAllProduct")]
        [OutputCache(Duration =86400)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_productService.GetAllProduct());
        }

    }
}