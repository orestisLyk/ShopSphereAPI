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
            CreateMap<ProductUpdateDTO, Product>();

            CreateMap<Category, CategoryReadOnlyDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();

            CreateMap<UserRegisterDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserReadOnlyDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<CartItemCreateDTO, CartItem>();
            CreateMap<CartItemUpdateDTO, CartItem>();   
            CreateMap<CartItem, CartItemReadOnlyDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Product.Sku))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Product.Price * src.Quantity));

            CreateMap<Cart, CartReadOnlyDTO>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
        }
    }
}
