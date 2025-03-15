using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using webapi_blazor.models.EbayDB;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EbayContext _context;
        private readonly IMapper _mapper;
        public OrderController(EbayContext db,IMapper mapper)
        {
            _context = db;
            _mapper = mapper;
        }



        [HttpPost("/Order/getAll")]
        public async Task<IActionResult> GetAllOrder()
        {
            DateTime fromDate = new DateTime(2024, 03, 01); // Ngày bắt đầu
            DateTime toDate = new DateTime(2024, 03, 10);   // Ngày kết thúc
           IEnumerable<GetListOrderDetailByOrderId> orderList = _context.GetListOrderDetailByOrderIds.Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate);

           
           IEnumerable<OrderItemVM> result =  _mapper.Map<IEnumerable<OrderItemVM>>(orderList);
            return Ok(result);
        }

    }
}