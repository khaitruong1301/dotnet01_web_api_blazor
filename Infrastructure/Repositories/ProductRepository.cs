using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi_blazor.models.EbayDB;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetAllAsync();
    public Task<Product?> GetByIdAsync(int id);
    public Task AddAsync(Product model);
    public Task Update(int id, Product modelUpdate);
    public void Update(Product modelUpdate);
}

public class ProductRepository : IProductRepository
{
    private readonly EbayContext _context;
    public ProductRepository(EbayContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<webapi_blazor.models.EbayDB.Product>> GetAllAsync()
    {
        IEnumerable<webapi_blazor.models.EbayDB.Product> products =  _context.Products.Skip(0).Take(10);
        return products;
    }

    
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Set<Product>().FindAsync(id);
    }
    public async Task AddAsync(Product model)
    {
        await _context.Set<Product>().AddAsync(model);
    }
    public async Task Update(int id, Product modelUpdate)
    {
        Product? prod = await GetByIdAsync(id);
        if (prod != null)
        {
            PropertyInfo[] lstProp = typeof(Product).GetProperties();
            foreach (PropertyInfo key in lstProp)
            {
                key.SetValue(prod, modelUpdate, null);
            }
        }
    }
    public void Update(Product modelUpdate)
    {
        _context.Set<Product>().Update(modelUpdate);
    }
    public async Task DeleteAsync(int id)
    {
        var model = await GetByIdAsync(id);
        if (model is not null)
        {
            _context.Set<Product>().Remove(model);
        }
    }
}