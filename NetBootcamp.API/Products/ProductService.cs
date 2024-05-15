using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.API.Products
{
    // Primary constructor
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository; // Dependency Injection

        // Method Injection using with "[FromServices]"
        public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalulatedTax(PriceCalculator priceCalculator)
        {
            var productList = _productRepository.GetAll().Select(product => new ProductDto
            (product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortTimeString())).ToImmutableList(); // immutable list geri yeni bir referans döner

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }

        public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null) return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            var newDto = new ProductDto(
                    hasProduct.Id,
                    hasProduct.Name,
                    priceCalculator.CalculateKdv(hasProduct.Price, 1.2m),
                    hasProduct.Created.ToShortDateString());

            return ResponseModelDto<ProductDto?>.Success(newDto);
        }

        public ResponseModelDto<int> Create(ProductCreateRequestDto request) // Add metotlarında geriye kaydedilen ürünün id si dönülür.
        {
            var newProduct = new Product
            {
                Id = _productRepository.GetAll().Count + 1,
                Name = request.Name,
                Price = request.Price,
                Created = DateTime.Now
            };

            _productRepository.Create(newProduct);

            return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        public ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = _productRepository.GetById(productId);

            if (hasProduct is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            var updatedProduct = new Product
            {
                Id = productId,
                Name = request.Name,
                Price = request.Price,
                Created = hasProduct.Created,
            };
            _productRepository.Update(updatedProduct);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Delete(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = GetByIdWithCalculatedTax(id, priceCalculator);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı", HttpStatusCode.NotFound);
            }

            _productRepository.Delete(id);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
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
