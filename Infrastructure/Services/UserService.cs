using System.Collections;
using System.Linq;
using webapi_blazor.models.EbayDB;
public interface IUserService
{
    public Task<dynamic> GetAllAsync();
}

public class UserService : IUserService
{
    public IUnitOfWork _uow;
    public UserService(IUnitOfWork uow) {
        _uow = uow;
    }
    public async Task<dynamic> GetAllAsync()
    {
        IEnumerable<webapi_blazor.models.EbayDB.User> res = await  _uow._userRepository.GetAllAsync();

        // webapi_blazor.models.EbayDB.User newUser = new webapi_blazor.models.EbayDB.User();
        // newUser.Email = "user@gmail.com";

        // await _uow._userRepository.AddAsync(newUser);


        // await _uow.SaveChangesAsync();



        return new {
            StatusCode= 200,
            Data = res
        };
    }
}