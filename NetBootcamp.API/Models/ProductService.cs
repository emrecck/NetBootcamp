using NetBootcamp.API.DTOs;
using System.Collections.Immutable;

namespace NetBootcamp.API.Models
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new();

        public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalulatedTax()
        {
            var productList = _productRepository.GetAll().Select(product => new ProductDto
            (product.Id, product.Name, CalculateKdv(product.Price, 1.2m), product.Created.ToShortTimeString())).ToImmutableList();

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }

        private decimal CalculateKdv(decimal price, decimal tax) => price * tax;

        public ProductDto? GetById(int id)
        {
            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null) return null!;

            return new ProductDto(
                hasProduct.Id, hasProduct.Name, CalculateKdv(hasProduct.Price, 1.2m), hasProduct.Created.ToShortDateString());
        }

        public ResponseModelDto<NoContent> Delete(int id)
        {
            var hasProduct = GetById(id);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı");
            }
            
            _productRepository.Delete(id);

            return ResponseModelDto<NoContent>.Success();
        }

        //public (bool isSuccess, string message) Delete(int id)    Tuple true, false yaklaşımı, success-fail, right-left yaklaşımı
        //{
        //    var hasProduct = GetById(id);

        //    if (hasProduct is null) return (false,"Silmeye çalıştığını ürün bulunamadı!"); 

        //    _productRepository.Delete(id);
        //    return (true, string.Empty);
        //}
    }
}
