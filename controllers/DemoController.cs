//api-controller-async
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_blazor.Models;
//using webapi_blazor.Models;

namespace webapi_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly StoreCybersoftContext _context = new StoreCybersoftContext();
        public DemoController()
        {
            // _context = db;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            List<User> lstUser = _context.Users.ToList();
            return Ok(lstUser);
        }
        [HttpGet("GetAllSQLRaw")]
        public async Task<ActionResult> GetAllSQLRaw()
        {
            List<User> lstUser = _context.Users.FromSqlRaw("Select * From Users").ToList();
            return Ok(lstUser);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser([FromBody] User newUser)
        {  
            //linq:
            _context.Users.Add(newUser);
            _context.SaveChanges(); //Lưu vào db thật
            return Ok(_context.Users.ToList());
        }
        [HttpPost("AddUserRaw")]
        public async Task<ActionResult> AddUserRaw([FromBody] User newUser)
        {  
            string sqlCommand = $"INSERT INTO Users(Name,Age,Email,Additional) values(N'{newUser.Name}','{newUser.Age}','{newUser.Email}','{newUser.Additional}')";
            //linq:
            _context.Database.ExecuteSqlRaw(sqlCommand);
            return Ok(_context.Users.ToList());
        }

        [HttpDelete("/delete/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute]int id)
        {
            User? usDelete = _context.Users.Find(id);
            if (usDelete != null){
                _context.Users.Remove(usDelete);
                _context.SaveChanges();
            }
            return Ok(_context.Users.ToList());
        }
        [HttpDelete("/deleteraw/{id}")]
        public async Task<ActionResult> Deleteraw([FromRoute]int id)
        {
            // _context.Database.BeginTransaction();
            string sqlCommand = $"DELETE FROM Users where Id={id}";
            //linq:
            _context.Database.ExecuteSqlRaw(sqlCommand);
            // _context.Database.RollbackTransaction();
            // _context.Database.CommitTransaction();
            return Ok(_context.Users.ToList());
        }


        [HttpPut("/edit/{id}")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody] User userEdit)
        {
            _context.Entry(userEdit).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(_context.Users.ToList());
        }

        [HttpPut("/update/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute]int id,[FromBody] User userEdit)
        {   
            //Lấy ra thằng user trong csdl
            User? userUpdate = _context.Users.Find(id);
            if(userUpdate!=null){
                userUpdate.Name = userEdit.Name;
                userUpdate.Age = userEdit.Age;
                userUpdate.Email = userEdit.Email;
                userUpdate.Additional = userEdit.Additional;
                _context.SaveChanges();
            }
            return Ok(_context.Users.ToList());
        }



        
    

    }
}