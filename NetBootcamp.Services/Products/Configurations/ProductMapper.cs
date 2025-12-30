using AutoMapper;
using NetBootcamp.Repository.Products;
using NetBootcamp.Services.Products.DTOs;
using NetBootcamp.Services.Products.Helpers;

namespace NetBootcamp.Services.Products.Configurations;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDto>()
            .ForPath(productDto => productDto.Created, opt => opt.MapFrom(product => product.Created.ToShortDateString()))
            .ForPath(productDto => productDto.Price, opt => opt.MapFrom(product => new PriceCalculator().CalculateKdv(product.Price, 1.2m)));
        CreateMap<ProductDto, Product>();
    }
}

