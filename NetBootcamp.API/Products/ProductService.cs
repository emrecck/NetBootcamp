using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.DTOs.ProductCreateUseCase;
using NetBootcamp.API.Redis;
using StackExchange.Redis;
using System.Collections.Immutable;
using System.Net;
using System.Text.Json;

namespace NetBootcamp.API.Products
{
    // Primary constructor
    public class ProductService(IProductRepository productRepository, RedisService redisService) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository; // Dependency Injection
        private const string ProductCacheKey = "products";
        private const string ProductCacheKeyAsList = "products-list";

        // Method Injection using with "[FromServices]"
        public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalulatedTax(PriceCalculator priceCalculator)
        {
            if (redisService.Database.KeyExists(ProductCacheKey))   // check redis
            {
                // Get datas from redis cache
                var productListFromRedisAsJson = redisService.Database.StringGet(ProductCacheKey);
                var productListFromRedis = JsonSerializer.Deserialize<ImmutableList<ProductDto>>(productListFromRedisAsJson!);
                return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListFromRedis!);
            }

            // Get datas from DB
            var productList = _productRepository.GetAll().Select(product => new ProductDto
            (product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortTimeString())).ToImmutableList(); // immutable list geri yeni bir referans döner

            #region  Set datas to Redis cache
            // set as string
            var productListAsJson = JsonSerializer.Serialize(productList);
            redisService.Database.StringSet(ProductCacheKey, productListAsJson);

            // set as list
            productList.ForEach(product =>
            {
                // product-list:1
                // product-list:2 ....
                redisService.Database.ListLeftPush($"{ProductCacheKeyAsList}:{product.Id}", JsonSerializer.Serialize(product));
            });

            // Set dictionary data as Hash
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "key1", "val1"},
                { "key2", "val2"}
            };

            foreach (var item in dict)
            {
                var entry = new HashEntry(item.Key, item.Value);
                redisService.Database.HashSet("hash-key", [entry]);
            }
            #endregion


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }

        public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
        {
            var customKey = $"{ProductCacheKeyAsList}:{id}";
            if (redisService.Database.KeyExists(customKey))
            {
                var productFromRedisAsJson = redisService.Database.ListGetByIndex(customKey, 0);
                var productFromRedis = JsonSerializer.Deserialize<ProductDto>(productFromRedisAsJson!);

                return ResponseModelDto<ProductDto?>.Success(productFromRedis);
            }

            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null) return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            // set redis cache
            redisService.Database.ListLeftPush($"{ProductCacheKeyAsList}:{id}", JsonSerializer.Serialize(hasProduct));

            var newDto = new ProductDto(
                    hasProduct.Id,
                    hasProduct.Name,
                    priceCalculator.CalculateKdv(hasProduct.Price, 1.2m),
                    hasProduct.Created.ToShortDateString());

            return ResponseModelDto<ProductDto?>.Success(newDto);
        }

        public ResponseModelDto<IReadOnlyList<ProductDto>> GetByPaging(int page, int pageSize, PriceCalculator priceCalculator)
        {
            var productList = _productRepository.GetByPaging(page, pageSize).Select(product => new ProductDto
            (product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortDateString())).ToImmutableList();

            return ResponseModelDto<IReadOnlyList<ProductDto>>.Success(productList);
        }

        public ResponseModelDto<int> Create(ProductCreateRequestDto request) // Add metotlarında geriye kaydedilen ürünün id si dönülür.
        {
            redisService.Database.KeyDelete(ProductCacheKey);   // CacheKey i siler

            // This validation moved to fluent validation validator.
            //
            //  var hasProduct = _productRepository.IsExist(request.Name);
            //  if (hasProduct)
            //    return ResponseModelDto<int>.Fail("Eklemek istediğiniz ürün zaten mevcut !");

            var newProduct = new Product
            {
                Id = _productRepository.GetAll().Count + 1,
                Name = request.Name.Trim(),
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

            redisService.Database.KeyDelete(ProductCacheKey);   // CacheKey i siler

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

        public ResponseModelDto<NoContent> UpdateProductName(ProductNameUpdateRequestDto request)
        {
            var hasProduct = _productRepository.GetById(request.Id);
            if (hasProduct is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            redisService.Database.KeyDelete(ProductCacheKey);   // CacheKey i siler

            _productRepository.UpdateProductName(request.Name, request.Id);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Delete(int id, PriceCalculator priceCalculator)
        {
            var hasProduct = GetByIdWithCalculatedTax(id, priceCalculator);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı", HttpStatusCode.NotFound);
            }

            redisService.Database.KeyDelete(ProductCacheKey);   // CacheKey i siler

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
