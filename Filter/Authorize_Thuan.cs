using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_blazor.models.EbayDB;
namespace webapi_blazor.Filter;
public class Authorize_Thuan : ActionFilterAttribute
{
    public EbayContext _context;
    public Authorize_Thuan(EbayContext db)
    {
        _context = db;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Lấy thông tin token từ header => giải mã token => Kiểm tra hợp lệ hay không 
        var id = context.HttpContext.Request.RouteValues["id"];
        var token = context.HttpContext.Request.Headers["token"].First();
        // var cookie = context.HttpContext.Request.Cookies["id"].First();
        var body = new StreamReader(context.HttpContext.Request.Body);


        var data = context.ActionArguments["us"];

        context.Result = new ContentResult(){
            Content = "Fail nhé con !",
            StatusCode = 401
        };
        
        var resultContext = await next(); // ⬅️ CHẠY ACTION ở đây

        context.HttpContext.Response.Cookies.Append("token", "abc123", new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

    }
    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        //Lấy thông tin token từ header => giải mã token => Kiểm tra hợp lệ hay không 
        // var id = context.HttpContext.Request.RouteValues["id"];
        // var token = context.HttpContext.Request.Headers["token"].First();
        // // var cookie = context.HttpContext.Request.Cookies["id"].First();
        // var body = new StreamReader(context.HttpContext.Request.Body);
        // var data =  await body.ReadToEndAsync();


    }



}