using Microsoft.AspNetCore.Mvc.Filters;
using webapi_blazor.models.EbayDB;
namespace webapi_blazor.Filter;
public class Authorize_Thuan : ActionFilterAttribute
{
    public EbayContext _context;
    public Authorize_Thuan(EbayContext db) {
        _context = db;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //Lấy thông tin token từ header => giải mã token => Kiểm tra hợp lệ hay không 

    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {

    }

  
}