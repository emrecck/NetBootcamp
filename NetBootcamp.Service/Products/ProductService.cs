using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Core.CrossCuttingConcerns.Caching;
using NetBootcamp.Repositories;
using NetBootcamp.Repositories.Entities.Products;
using NetBootcamp.Service.Products.DTOs;
using NetBootcamp.Service.Products.Helpers;
using NetBootcamp.Service.Products.ProductCreateUseCase;
using NetBootcamp.Service.SharedDTOs;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.Service.Products
{
    // Primary constructor
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, CacheService cacheService) : IProductService
    {
        //private readonly IProductRepository productRepository = productRepository; // Dependency Injection

        // Method Injection using with "[FromServices]"
        public async Task<ResponseModelDto<IImmutableList<ProductDto>>> GetAllWithCalulatedTax(PriceCalculator priceCalculator)
        {
            string cacheKey = cacheService.CreateCacheKey("ProductService", "GetAllWithCalculatedTax", "ProductDtos");

            IImmutableList<ProductDto> data;
            var isExist = cacheService.IsExistCache(cacheKey, out data);
            if (isExist)
                return ResponseModelDto<IImmutableList<ProductDto>>.Success(data);

            var productList = await productRepository.GetAll();
            var productDtoList = mapper.Map<List<ProductDto>>(productList); // immutable list geri yeni bir referans döner
            cacheService.AddCache(cacheKey, productDtoList);

            return ResponseModelDto<IImmutableList<ProductDto>>.Success(productDtoList.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = await productRepository.GetById(id);

            if (hasProduct is null) return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            var newDto = mapper.Map<ProductDto>(hasProduct);

            return ResponseModelDto<ProductDto?>.Success(newDto);
        }

        public async Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request) // Add metotlarında geriye kaydedilen ürünün id si dönülür.
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Barcode = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now
            };

            var addProduct = await productRepository.Add(newProduct);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<int>.Success(addProduct.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = await productRepository.GetById(productId);

            if (hasProduct is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            var updatedProduct = new Product
            {
                Id = productId,
                Name = request.Name,
                Price = request.Price,
                CreatedDate = hasProduct.CreatedDate,
            };
            await productRepository.Update(updatedProduct);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = await productRepository.GetById(id);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı", HttpStatusCode.NotFound);
            }

            await productRepository.Delete(hasProduct);
            await unitOfWork.CommitAsync();
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
