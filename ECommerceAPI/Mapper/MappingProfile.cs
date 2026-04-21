using AutoMapper;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs.Auth;
using ECommerceAPI.DTOs.Category;
using ECommerceAPI.DTOs.Products;
using ECommerceAPI.DTOs.Cart;
using ECommerceAPI.DTOs.Orders;
using ECommerceAPI.DTOs.OTP;
using ECommerceAPI.DTOs.Wallet;

namespace ECommerceAPI.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {         
            CreateMap<CreateCartItemDTO, CartItem>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.customerId))
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.productId))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.quantity));
            
            CreateMap<CartItem, CartItemResponseDTO>()
                .ForMember(dest => dest.customerId, 
                    opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.productId, 
                    opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.quantity,
                    opt => opt.MapFrom(src => src.Quantity));
            
            CreateMap<CartItemResponseDTO, CartItem>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.customerId))
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.productId))
                .ForMember(dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.quantity));

            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.name));

            CreateMap<Category, CategoryResponseDTO>()
                .ForMember(dest => dest.name,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<Product, ProductResponseDTO>()
                .ForMember(dest => dest.id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.price,
                    opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.stock,
                    opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.categoryId,
                    opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.category,
                    opt => opt.MapFrom(src => src.Category));

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Stock,
                    opt => opt.MapFrom(src => src.stock))
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.category));

            CreateMap<CreateProductV2DTO, Product>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Stock,
                    opt => opt.MapFrom(src => src.stock))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.categoryId));

            CreateMap<Wallet, WalletResponseDTO>()
                .ForMember(dest => dest.customerId,
                    opt => opt.MapFrom(src => src.customerId))
                .ForMember(dest => dest.amount,
                    opt => opt.MapFrom(src => src.amount));
        }

    }
}
