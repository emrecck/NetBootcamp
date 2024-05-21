using AutoMapper;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.Helpers;

namespace NetBootcamp.API.Products.Configurations
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(productDto => productDto.Created, opt => opt.MapFrom(product => product.Created.ToShortDateString()))
                .ForMember(productDto => productDto.Price, opt => opt.MapFrom(product => new PriceCalculator().CalculateKdv(product.Price, 1.2m)));
            CreateMap<ProductDto, Product>();
        }
    }
}
