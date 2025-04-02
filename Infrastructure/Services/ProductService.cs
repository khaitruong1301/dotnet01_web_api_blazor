//Đảm nhiệm chức năng chính của ứng
using Microsoft.AspNetCore.Mvc;
using webapi_blazor.models.EbayDB;

public interface IProductService
{
    //Có CRUD và những nghiệp vụ khác liên quan đến nhiều table khác của ứng dụng
    Task<dynamic> GetAllProduct();

}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    public async Task<dynamic> GetAllProduct()
    {
        var products = await _unitOfWork._productRepository.GetAllAsync();
        // await _unitOfWork.SaveChangesAsync(); 
        return new {
            StatusCode = 200,
            Data = products.Skip(0).Take(5000)
        };
    }
}



