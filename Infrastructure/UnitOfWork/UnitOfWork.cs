// unitofwork
using webapi_blazor.models.EbayDB;

public interface IUnitOfWork : IAsyncDisposable
{
    public IProductRepository _productRepository{get;} //Có thì 
    public IUserRepository _userRepository{get;} //Có thì sẽ dễ quản lý
    // sẽ dễ quản lý
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{
    public IProductRepository _productRepository{get;}
    public IUserRepository _userRepository{get;}
    

    private readonly EbayContext _context;
    
    public UnitOfWork(EbayContext context, IProductRepository productRepository, IUserRepository userRepository) 
    {
        _context = context;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}

