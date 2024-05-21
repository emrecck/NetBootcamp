using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.Helpers;
using NetBootcamp.API.Products.ProductCreateUseCase;
using System.Collections.Immutable;

namespace NetBootcamp.API.Products.Async
{
    public interface IProductServiceAsync
    {
        Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalulatedTaxAsync(PriceCalculator priceCalculator);
        Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTaxAsync(int id, PriceCalculator priceCalculator);
        Task<ResponseModelDto<IReadOnlyList<ProductDto>>> GetByPagingAsync(int page, int pageSize, PriceCalculator priceCalculator);
        Task<ResponseModelDto<int>> CreateAsync(ProductCreateRequestDto request);
        Task<ResponseModelDto<NoContent>> UpdateAsync(int productId, ProductUpdateRequestDto request);
        Task<ResponseModelDto<NoContent>> UpdateProductNameAsync(ProductNameUpdateRequestDto request);
        Task<ResponseModelDto<NoContent>> DeleteAsync(int id, PriceCalculator priceCalculator);
    }
}
