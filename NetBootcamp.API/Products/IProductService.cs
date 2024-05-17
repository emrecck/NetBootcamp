using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.DTOs.ProductCreateUseCase;
using System.Collections.Immutable;

namespace NetBootcamp.API.Products
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
