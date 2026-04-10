using AutoMapper;
using PizzaDelivery.API.DTOs;
using PizzaDelivery.API.Models;

namespace PizzaDelivery.API.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product Mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();

        // Order Mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateOrderDto, Order>();
        CreateMap<OrderItem, OrderItemDto>();

        // User Mappings
        CreateMap<User, UserDto>();
        CreateMap<UserRegisterDto, User>();

        // Promotion Mappings
        CreateMap<Promotion, PromotionDto>();
        CreateMap<CreatePromotionDto, Promotion>();
    }
}
