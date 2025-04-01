using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
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
        public IDistributedCache _cacheRedis;
        public IConnectionMultiplexer _redisServer;
        public IDatabase _dbRedis;
        public RedisHelper _redisHelper;
        
        public DemoCacheController(EbayContext db, IMemoryCache cache,IDistributedCache redisCache ,IConnectionMultiplexer redis,RedisHelper  redisHelper)
        {
            _context = db;
            _cache = cache;
            _cacheRedis = redisCache;
            _redisServer = redis;
            _dbRedis = _redisServer.GetDatabase(0); // từ 0 đến 15
            _redisHelper = redisHelper;
            
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

        [HttpGet("GetAllUser")]
        [OutputCache(Duration = 60)]
        public async Task<ActionResult> GetAllUser()
        {
            var lstUser = _context.Users.AsNoTracking().Select(n => new {
                Id = n.Id,
                Name = n.FullName
            });
            return Ok(new {
                StatusCode = 200,
                Data = lstUser
            });
        }
        

        [HttpGet("GetAllProductRedis")]
        public async Task<IActionResult> GetAllProductRedis()
        {
            string cacheKey = "product_list";
            var data = await _cacheRedis.GetStringAsync(cacheKey);
            if(data == null){
                //Nếu chưa có dữ liệu thì truy vấn db lấy dữ liệu ra và lưu vào cache
                var prodList = await _context.Products.Skip(0).Take(5000).ToListAsync();
                string dataCache = JsonSerializer.Serialize(prodList);
                //Lưu vào cache
                await _cacheRedis.SetStringAsync(cacheKey, dataCache, new DistributedCacheEntryOptions () {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) //cache 1 ngày
                });
                return Ok(prodList);
            }
            //Nếu đã có trong cache thì lấy từ cache ra
            var res = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
            return Ok(res);
        }

        [HttpGet("GetAllProductRedisDB")]
        public async Task<IActionResult> GetAllProductRedisDB()
        {
            string cacheKey = "product_list_db";
            var data = await _dbRedis.StringGetAsync(cacheKey);
            if(string.IsNullOrEmpty(data)){
                //Nếu chưa có dữ liệu thì truy vấn db lấy dữ liệu ra và lưu vào cache
                var prodList = await _context.Products.Skip(0).Take(5000).ToListAsync();
                string dataCache = JsonSerializer.Serialize(prodList);
                //Lưu vào cache
                await _dbRedis.StringSetAsync(cacheKey, dataCache, TimeSpan.FromDays(1));
                return Ok(prodList);
            }
            //Nếu đã có trong cache thì lấy từ cache ra
            var res = JsonSerializer.Deserialize<IEnumerable<Product>>(data);
            return Ok(res);
        }
         [HttpGet("GetAllProductRedisHelper")]
        public async Task<IActionResult> GetAllProductRedisHelper()
        {
            _redisHelper.setDatabaseRedis(1);
            string cacheKey = "product_list_db";
            var data = await _redisHelper.GetAsync<IEnumerable<Product>>(cacheKey);
            if(data == null){
                var productList =  await _context.Products.Skip(0).Take(5000).ToListAsync();
                await _redisHelper.SetAsync(cacheKey,productList, TimeSpan.FromDays(1));
            }
            return Ok(data);
        }
    }
}