using NetBootcamp.Service.Products.DTOs;
using NetBootcamp.Service.Products.Helpers;
using NetBootcamp.Service.Products.ProductCreateUseCase;
using NetBootcamp.Service.SharedDTOs;
using System.Collections.Immutable;

namespace NetBootcamp.Service.Products
{
    public interface IProductService
    {
        Task<ResponseModelDto<IImmutableList<ProductDto>>> GetAllWithCalulatedTax(PriceCalculator priceCalculator);
        Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);
        Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request);
        Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request);
        Task<ResponseModelDto<NoContent>> Delete(int id, PriceCalculator priceCalculator);
    }
}
