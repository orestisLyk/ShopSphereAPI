using AutoMapper;
using ShopSphere.DTO;
using ShopSphere.Model;

namespace ShopSphere.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, ProductMinimalDTO>();
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl).ToList()));
            CreateMap<ProductCreateDTO, Product>();
        }
    }
}
