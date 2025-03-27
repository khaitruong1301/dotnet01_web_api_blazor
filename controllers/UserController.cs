//api-controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using webapi_blazor.Helper;
using webapi_blazor.models.EbayDB;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EbayContext _context;
        private readonly JwtAuthService _jwtService;
        public UserController(EbayContext db, JwtAuthService jwt)
        {
            _context = db;
            _jwtService = jwt;
        }

        [HttpPost("/user/login")]
        public async Task<ActionResult> Login(UserLoginVM userLogin)
        {
            //Tìm user trong db có username hoặc email 
            var userCheckLogin = await _context.Users.SingleOrDefaultAsync(us => us.Username == userLogin.Account || us.Email == userLogin.Account);
            if (userCheckLogin != null && PasswordHelper.VerifyPassword(userLogin.Password, userCheckLogin.PasswordHash)) //Nếu account có trong db (account có thể username hoặc email)    
            {

                //Tạo token
                string token = _jwtService.GenerateToken(userCheckLogin);
                UserLoginResultVM usLoginResult = new UserLoginResultVM();
                usLoginResult.AccessToken = token;
                usLoginResult.Account = userLogin.Account;
                return Ok(usLoginResult);
                //Trả về kết quả là user name và token
            }
            return BadRequest("Tài khoản mật khẩu không đúng!");
        }
        [Authorize]
        [OutputCache(Duration = 60, VaryByHeaderNames = new[] { "Authorization" })]
        [HttpGet("/user/GetProfile")]
        public async Task<ActionResult> GetProfile([FromHeader] string authorization)
        {
            string token = authorization.Replace("Bearer ", "");
            // string token  = HttpContext.Request.Headers["Authorization"];
            string account = _jwtService.DecodePayloadToken(token);
            var user = _context.Users.SingleOrDefault(us => us.Username == account || us.Email == account);
            return Ok(user);
        }


        [Authorize(Roles = "Buyer,Seller")]
        [HttpGet("/user/PostNew")]
        public async Task<ActionResult> PostNew()
        {

            return Ok("");
        }




        [HttpGet("product/{id}")]
        [OutputCache(Duration = 30, VaryByRouteValueNames = new[] { "id" })]
        public IActionResult GetProduct(int id)
        {
            return Ok($"Product {id} - {DateTime.Now}");
        }

        [HttpGet("search")]
        [OutputCache(Duration = 60, VaryByQueryKeys = new[] { "q", "page" })]
        public IActionResult Search(string q, int page)
        {
            return Ok($"Search: {q}, page {page}, time: {DateTime.Now}");
        }



    }

}



