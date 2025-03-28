using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace webapi_blazor.Filter;
public class DemoFilter : ActionFilterAttribute
{
    public string abc{get;set;} ="";
    public DemoFilter() {

    }
    //  protected override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutingContext filterContext)
    // {
    //     string? username = context.HttpContext.Request.Form["Username"];

    // }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Headers["Authorization"];

        string cookie = context.HttpContext.Request.Cookies["token"];
        // context.Result = new ContentResult(){
        //     StatusCode = 401,
        //     Content = "Unauthorize"
        // };

    

    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
            string token = context.HttpContext.Request.Headers["Authorization"];
            context.HttpContext.Response.Cookies.Append("token",token, new CookieOptions(){
            Expires = DateTime.Now.AddDays(30),
            HttpOnly = true
        } );

    }

  
}