using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.Helpers;
using NetBootcamp.API.Products.ProductCreateUseCase;
using NetBootcamp.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.API.Products.Syncs
{
    // Primary constructor
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        // Method Injection using with "[FromServices]"
        public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalulatedTax(PriceCalculator priceCalculator)
        {
            var productList = productRepository.GetAll().Select(product => new ProductDto
            (product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortTimeString(), product.Barcode, product.Stock)).ToImmutableList(); // immutable list geri yeni bir referans döner

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }

        public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = productRepository.GetById(id);

            //if (hasProduct is null) return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            var newDto = new ProductDto(
                    hasProduct.Id,
                    hasProduct.Name,
                    priceCalculator.CalculateKdv(hasProduct.Price, 1.2m),
                    hasProduct.Created.ToShortDateString(),
                    hasProduct.Barcode,
                    hasProduct.Stock);

            return ResponseModelDto<ProductDto?>.Success(newDto);
        }

        public ResponseModelDto<IReadOnlyList<ProductDto>> GetByPaging(int page, int pageSize, PriceCalculator priceCalculator)
        {
            var productList = productRepository.GetByPaging(page, pageSize).Select(product => new ProductDto
            (product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortDateString(), product.Barcode, product.Stock)).ToImmutableList();

            return ResponseModelDto<IReadOnlyList<ProductDto>>.Success(productList);
        }

        public ResponseModelDto<int> Create(ProductCreateRequestDto request) // Add metotlarında geriye kaydedilen ürünün id si dönülür.
        {
            // This validation moved to fluent validation validator.
            //
            //  var hasProduct = productRepository.IsExist(request.Name);
            //  if (hasProduct)
            //    return ResponseModelDto<int>.Fail("Eklemek istediğiniz ürün zaten mevcut !");

            var newProduct = new Product
            {
                Name = request.Name.Trim(),
                Price = request.Price,
                Created = DateTime.Now,
                Barcode = Guid.NewGuid().ToString(),
                Stock = request.Stock,
            };

            productRepository.Create(newProduct);

            unitOfWork.Commit();
            return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        public ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = productRepository.GetById(productId);

            //if (hasProduct is null)
            //    return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            //var updatedProduct = new Product
            //{
            //    Id = productId,
            //    Name = request.Name,
            //    Price = request.Price,
            //    Created = hasProduct.Created,
            //    Barcode = hasProduct.Barcode,
            //    Stock = request.Stock
            //};

            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;
            hasProduct.Stock = request.Stock;

            productRepository.Update(hasProduct);

            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> UpdateProductName(ProductNameUpdateRequestDto request)
        {
            var hasProduct = productRepository.GetById(request.Id);
            if (hasProduct is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            productRepository.UpdateProductName(request.Name, request.Id);

            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Delete(int id, PriceCalculator priceCalculator)
        {
            //var hasProduct = GetByIdWithCalculatedTax(id, priceCalculator);
            //if (hasProduct is null)
            //{
            //    return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı", HttpStatusCode.NotFound);
            //}

            productRepository.Delete(id);

            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        //public (bool isSuccess, string message) Delete(int id)    Tuple true, false yaklaşımı, success-fail, right-left yaklaşımı
        //{
        //    var hasProduct = GetById(id);

        //    if (hasProduct is null) return (false,"Silmeye çalıştığını ürün bulunamadı!"); 

        //    productRepository.Delete(id);
        //    return (true, string.Empty);
        //}


    }
}
