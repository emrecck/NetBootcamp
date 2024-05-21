using NetBootcamp.Services.Products.DTOs;
using NetBootcamp.Services.Products.Helpers;
using NetBootcamp.Services.Products.ProductCreateUseCase;
using NetBootcamp.Services.SharedDTOs;
using System.Collections.Immutable;

namespace NetBootcamp.Services.Products.Syncs
{
    public interface IProductService
    {
        ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalulatedTax(PriceCalculator priceCalculator);
        ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);
        ResponseModelDto<IReadOnlyList<ProductDto>> GetByPaging(int page, int pageSize, PriceCalculator priceCalculator);
        ResponseModelDto<int> Create(ProductCreateRequestDto request);
        ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request);
        ResponseModelDto<NoContent> UpdateProductName(ProductNameUpdateRequestDto request);
        ResponseModelDto<NoContent> Delete(int id, PriceCalculator priceCalculator);
    }
}
