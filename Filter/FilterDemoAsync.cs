using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_blazor.models.EbayDB;
namespace webapi_blazor.Filter;
public class FilterDemoAsync : ActionFilterAttribute
{
    public FilterDemoAsync() { 

    }
    // Đây là hàm đầy đủ override từ ActionFilterAttribute
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Trước khi thực thi action
        // _logger.LogInformation("➡️ [Before] Executing action: {ActionName}", context.ActionDescriptor.DisplayName);
        Console.WriteLine("trước khi action thực thi");
        // Gọi action tiếp theo trong pipeline
        var resultContext = await next(); //Nếu muốn xử lý sau khi action thực thi xong thì thêm vào
        Console.WriteLine("Đã thực thi xong");
     
    }

}
