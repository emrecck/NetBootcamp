using AutoMapper;
using NetBootcamp.Repositories.Entities.Products;
using NetBootcamp.Service.Products.DTOs;
using NetBootcamp.Service.Products.Helpers;

namespace NetBootcamp.Service.Products.Configurations
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForPath(x=>x.Price, opt => opt.MapFrom(y => PriceCalculator.CalculateKdv(y.Price, 1.2m)))
                .ForPath(x => x.CreatedDate, opt => opt.MapFrom(y => y.CreatedDate.ToShortDateString())).ReverseMap();
        }
    }
}
