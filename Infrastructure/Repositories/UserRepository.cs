using System.Linq.Expressions;
using webapi_blazor.models.EbayDB;

public interface IUserRepository : IRepository<webapi_blazor.models.EbayDB.User>
{

    //Nếu muốn định nghĩa thêm thì liệt kê
    public string otherFunction();
}

public class UserRepository : Repository<webapi_blazor.models.EbayDB.User>,IUserRepository
{
    public UserRepository(EbayContext context): base(context)
    {
    }
    public string otherFunction()
    {
       return "handle other function";
    }
}


