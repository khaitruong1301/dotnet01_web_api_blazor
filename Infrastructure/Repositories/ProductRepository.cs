using Microsoft.EntityFrameworkCore;
using webapi_blazor.models.EbayDB;

public interface IProductRepository 
{
    Task AddProduct (Product newProduct);
    Task<IEnumerable<Product>> GetAllProducts();
}

public class ProductRepository : IProductRepository
{
    private readonly EbayContext _context;
    public ProductRepository(EbayContext context) 
    {
        _context = context;
    }
    public async Task AddProduct(Product newProduct)
    {
        await _context.Set<Product>().AddAsync(newProduct);
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Set<Product>().ToListAsync();
    }
}