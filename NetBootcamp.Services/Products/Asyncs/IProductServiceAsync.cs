using NetBootcamp.Services.Products.DTOs;
using NetBootcamp.Services.Products.Helpers;
using NetBootcamp.Services.Products.ProductCreateUseCase;
using NetBootcamp.Services.SharedDTOs;
using System.Collections.Immutable;

namespace NetBootcamp.Services.Products.Asyncs
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
