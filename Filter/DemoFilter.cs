using Microsoft.AspNetCore.Mvc.Filters;
namespace webapi_blazor.Filter;
public class DemoFilter : ActionFilterAttribute
{
    public string abc{get;set;} ="";
    public DemoFilter() {

    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string? username = context.HttpContext.Request.Form["Username"];

    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {

    }

  
}