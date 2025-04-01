using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using webapi_blazor.models.EbayDB;
//using webapi_blazor.Models;


namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoCacheController : ControllerBase
    {
        private readonly EbayContext _context;
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _dbRedis;


        public DemoCacheController(IDistributedCache cache, EbayContext context, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _context = context;
            _dbRedis = redis.GetDatabase(0);
            _redis = redis;
        }

        [HttpGet("SetGetStringDataRedisDB")]
        public async Task<IActionResult> SetGetStringDataRedisDB()
        {
            await _dbRedis.StringSetAsync("cybersoft", "hello");
            var val = await _dbRedis.StringGetAsync("cybersoft");
            return Ok(new
            {
                data = val.ToString()

            });
        }

        [HttpGet("SetHashDataRedisDB")]
        public async Task<IActionResult> SetHashDataRedisDB()
        {
            //Khi lưu bằng hash thì dữ liệu sẽ bị đè nếu trùng key
            await _dbRedis.HashSetAsync("user:123", new HashEntry[]
            {
                    new("name", "John"),
                    new("email", "john@example.com")
            });

            return Ok(new
            {
                name =  _dbRedis.HashGet("user:123", "name").ToString(),
                email = _dbRedis.HashGet("user:123", "email").ToString(),
            });
        }
        

        [HttpGet("TriggerSnapshotAsync")]
        public async Task<IActionResult> TriggerSnapshotAsync()
        {
            IServer server = GetRedisServer();
            await server.ExecuteAsync("BGSAVE"); // Gọi BGSAVE
            Console.WriteLine("✅ Redis snapshot (BGSAVE) đã được kích hoạt.");
            return Ok("ok");
        }


        private IServer GetRedisServer()
        {
            var endpoint = _redis.GetEndPoints().First();
            return _redis.GetServer(endpoint);

        }





        [HttpGet("GetDataAsync")]
        public async Task<IActionResult> GetDataAsync()
        {
            var cacheKey = "productList";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (cachedData != null)
            {
                var res = JsonSerializer.Deserialize<IEnumerable<Product>>(cachedData);
                return Ok(res);
            }
            // Giả lập lấy dữ liệu tốn thời gian
            var data = _context.Products.Skip(0).Take(5000);
            string freshData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(cacheKey, freshData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            });

            return Ok(data);
        }
    }
}
