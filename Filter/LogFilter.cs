using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using webapi_blazor.models.EbayDB;
namespace webapi_blazor.Filter;
public class LogFilter : ActionFilterAttribute
{
    public EbayContext _context;
    
    public ILogger<LogFilter> Logger { get; set;}
    private string pathName = "Logs/log.txt";

    public LogFilter(EbayContext db, ILogger<LogFilter> log)
    {
        _context = db;
        Logger = log;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string ip = context.HttpContext.Connection.RemoteIpAddress.ToString() ;
        File.AppendAllText(pathName,$@"ip connect: {ip} - {DateTime.Now.ToString()}" );
        var connect = _context.ConnectionCountLogs.SingleOrDefault(item => item.IpAddress == ip && DateTime.Now.Date == item.CreatedAt.Value.Date);
        if(connect != null){
            if(connect.ConnectionCount >= 1000 ){
                context.Result = new ContentResult(){
                    StatusCode = 403,
                    Content = "Forbiden"
                };
            }else {
                connect.ConnectionCount += 1;
                _context.SaveChanges();
            }
        }else {
            ConnectionCountLog conn = new ConnectionCountLog();
            conn.IpAddress = ip;
            conn.ConnectionCount = 1;
            conn.ConnectionTime = DateTime.Now;
            conn.CreatedAt = DateTime.Now;
            _context.ConnectionCountLogs.Add(conn);
            _context.SaveChanges();
        }
        // ConnectionCountLog conn = new ConnectionCountLog();
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
    }

}