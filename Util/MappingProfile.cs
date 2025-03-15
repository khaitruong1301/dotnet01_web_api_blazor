using AutoMapper;
using webapi_blazor.models.EbayDB;
using System.Text.Json;
using webapi_blazor.Models.ViewModel;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Ánh xạ từ User sang UserDTO
        // CreateMap<GetListOrderDetailByOrderId, OrderItemVM>()
        //         .ForMember(
        //             dest => dest.lstOrderDetail,
        //             opt => opt.MapFrom(src => JsonSerializer.Deserialize<List<ItemOrderVM>>(src.OrderDetail, new JsonSerializerOptions()))
        //         ).ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<GetListOrderDetailByOrderId, OrderItemVM>()
        .ConvertUsing(dto => new OrderItemVM
        {
            lstOrderDetail = JsonSerializer.Deserialize<List<ItemOrderVM>>(dto.OrderDetail, new JsonSerializerOptions()),
            Id = dto.Id,
            CreatedAt = dto.CreatedAt,
            // OrderDetail sẽ tự động không được ánh xạ vì không gán
        });



            CreateMap<ProductDetailVM, ProductDetailResultVM>().ForMember(dest => dest.ListImage,
                    opt => opt.MapFrom(src => JsonSerializer.Deserialize<List<ImageUrlVM>>(src.ListImage, new JsonSerializerOptions())));

        //     CreateMap<ProductDetailVM, ProductDetailResultVM>()
        // .ConvertUsing(dto => new ProductDetailResultVM
        // {
        //     Id = dto.Id,
        //     Name= dto.Name,
        //     Category = dto.Category,
        //     Description = dto.Description,
        //     Price = dto.Price,
        //     CreatedAt = dto.CreatedAt,
        //     ListImage = JsonSerializer.Deserialize<List<ImageUrlVM>>(dto.ListImage, new JsonSerializerOptions())
        // });

        // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        // Ánh xạ ngược lại từ UserDTO về User
        //   CreateMap<UserDTO, UserEntity>()
        //     .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Không ánh xạ CreatedAt
    }
}
