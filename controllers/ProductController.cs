//Xây dựng 2 api (get all product và get Product by id)
//getallproduct => 2 param : page index (số trang) - pageSize(số phần tử trên 1 trang) => ví dụ người dùng truyền pageindex = 0 -> pageSize= 10 => dòng 0 -> 10 .Skip(0).Take(10);
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_blazor.models.EbayDB;
using webapi_blazor.Models.ViewModel;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EbayContext _context;
        private readonly IMapper _mapper;
        public ProductController(EbayContext db, IMapper mapper)
        {
            _context = db;
            _mapper = mapper;
        }
        


        [Authorize(Roles ="Seller")]
        [HttpGet("/product/getall")]
        public async Task<IActionResult> getAll(int pageIndex = 0, int pageSize = 10)
        {
            //linq
            var productList = _context.Products.Skip(pageIndex * pageSize).Take(pageSize);
            return Ok(productList);
        }

        [HttpGet("/product/getallsqlraw")]
        public async Task<IActionResult> getAllSQLRaw(int pageIndex = 0, int pageSize = 10)
        {
            int pageNumber = 0;
            bool isNumber = int.TryParse(pageIndex.ToString(), out pageNumber);
            if (isNumber)
            {
                string sqlCommand = $"select * from products order by id offset  {pageNumber * pageSize} rows fetch next {pageSize} rows only;";
                //linq
                var productList = _context.Products.FromSqlRaw(sqlCommand).ToList();
                return Ok(productList);
            }
            return BadRequest("Tham số không hợp lệ !");
        }

        [HttpGet("/product/getProductListCategory")]
        public async Task<IActionResult> getProductListCategory()
        {

            return Ok(_context.ProductListCategories.Skip(0).Take(10));
        }

        [HttpGet("/product/getbyid/{id}")]
        public async Task<IActionResult> getById([FromRoute] int id)
        {
            var productDetail = _context.Products.SingleOrDefault(prod => prod.Id == id);
            if (productDetail != null)
            {
                return Ok(productDetail);
            }
            return BadRequest("mã sản phẩm không tồn tại");
        }

        [HttpGet("/GetDetailProductById/{id}")]
        public async Task<ActionResult> GetDetailProductById(int id)
        {
            // var result = await _context.Set<ProductDetailVM>().FromSqlRaw($@"EXEC GetProductDetailById {id}").ToListAsync();
            var result = await _context.Database.SqlQueryRaw<ProductDetailVM>($@"EXEC GetProductDetailImageListById {id}").ToListAsync();
            #region  groupby
            //   List<ProductDetailVM> res = new List<ProductDetailVM>();
            // var resGroup = result.GroupBy(key => new
            // {
            //     Id = key.Id,
            //     Name = key.Name,
            //     Category = key.Category,
            //     Price = key
            // .Price,
            //     CreateAt = key.CreatedAt
            // }).ToList();
            // List<string> lstImg = new List<string>();
            // foreach (var item in resGroup)
            // {
            //     foreach (var img in item)
            //     {
            //         lstImg.Add(img.ListImage);
            //     }
            // }
            // ProductDetailVM resFinal = new ProductDetailVM();
            // resFinal.Id = resGroup.First().Key.Id;
            // resFinal.Name = resGroup.First().Key.Name;
            // resFinal.Price = resGroup.First().Key.Id;
            // resFinal.ListImage = lstImg.ToString();;
            #endregion
            if (result.Count() == 0)
            {
                return BadRequest("id không tồn tại");
            }
            var res = _mapper.Map<ProductDetailResultVM>(result.FirstOrDefault());
            return Ok(res);
        }

        
        [HttpGet("/GetAllUserRole")]
        public async Task<IActionResult> GetAllUserRole() {
            var lstUserRole = _context.UserRoles.Include(p => p.Role).Select(ul => new {
                id = ul.UserId,
                Role = ul.Role
            });
            return Ok(lstUserRole);
        }


    }
}