using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using webapi_blazor.models.EbayDB;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoCacheController : ControllerBase
    {   
        public EbayContext _context;
        public IMemoryCache _cache;
        public DemoCacheController(EbayContext db, IMemoryCache cache)
        {
            _context = db;
            _cache = cache;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            string cacheKey = "productList";
            IEnumerable<Product>?  products = _cache.Get<IEnumerable<Product>>(cacheKey);
            if(products == null){
                //Nếu chưa tồn tại trong cache => truy vấn db 
                products = await _context.Products.Skip(0).Take(5000).ToListAsync();

                var cacheOption = new MemoryCacheEntryOptions();
                cacheOption.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                // TimeSpan tp = DateTime.Now.AddDays(1) - DateTime.Now;
                //Lưu giá trị vào cache
                _cache.Set(cacheKey, products,cacheOption);
            }
            return Ok(products);
        }

        [HttpGet("ClearCache")]
        public async Task<IActionResult> ClearCache()
        {
            string cacheKey = "productList";
            _cache.Remove(cacheKey);

            return Ok("ok");
        }

    }
}