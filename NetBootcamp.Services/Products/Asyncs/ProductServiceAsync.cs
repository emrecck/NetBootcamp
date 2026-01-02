using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Repository.Products;
using NetBootcamp.Repository.Products.Asyncs;
using NetBootcamp.Repository.Repositories;
using NetBootcamp.Services.Products.DTOs;
using NetBootcamp.Services.Products.Helpers;
using NetBootcamp.Services.Products.ProductCreateUseCase;
using NetBootcamp.Services.Redis;
using NetBootcamp.Services.SharedDTOs;
using System.Collections.Immutable;
using System.Net;
using System.Text.Json;

namespace NetBootcamp.Services.Products.Asyncs
{
    // Primary constructor
    public class ProductServiceAsync(RedisService redisService, IProductRepositoryAsync productRepositoryAsync, IUnitOfWork unitOfWork, IMapper mapper) : IProductServiceAsync
    {
        private const string productCacheKey = "product";
        private const string productListCacheKey = "product-list";
        public async Task<ResponseModelDto<int>> CreateAsync(ProductCreateRequestDto request)
        {
            // create işleminde cache i siliyoruz
            redisService.Database.KeyDelete(productListCacheKey);
            var newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Barcode = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
            };

            var createdEntity = await productRepositoryAsync.CreateAsync(newProduct);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<int>.Success(createdEntity.Id);
        }

        public async Task<ResponseModelDto<NoContent>> DeleteAsync(int id, PriceCalculator priceCalculator)
        {
            await productRepositoryAsync.DeleteAsync(id);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalulatedTaxAsync(PriceCalculator priceCalculator)
        {
            #region StringSet
            //if (redisService.Database.KeyExists(productCacheKey))
            //{
            //    var productListAsJsonToGet = redisService.Database.StringGet(productCacheKey).ToString();
            //    var productListFromCache = JsonSerializer.Deserialize<ImmutableList<ProductDto>>(productListAsJsonToGet);
            //    return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListFromCache);
            //}


            //var productList = await productRepositoryAsync.GetAllAsync();

            // string olarak cache te tutma
            //var productListAsJsonToSet = JsonSerializer.Serialize(productList);
            //redisService.Database.StringSet(productCacheKey, productListAsJsonToSet, TimeSpan.FromMinutes(10));
            #endregion
            
            #region ListSet
            
            if (redisService.Database.KeyExists(productListCacheKey))
            {
                var productListAsJsonToGet = await redisService.Database.ListRangeAsync(productListCacheKey, 0, -1);
                var productListFromCache = productListAsJsonToGet
                    .Select(x => JsonSerializer.Deserialize<ProductDto>(x.ToString()))
                    .Where(x => x is not null)
                    .ToImmutableList()!;

                return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListFromCache);
            }

            var productList = await productRepositoryAsync.GetAllAsync();

            // List olarak cache te tutma
            productList.ToList().ForEach((item) =>
            {
                redisService.Database.ListLeftPushAsync(productListCacheKey, JsonSerializer.Serialize(item));
            });
            #endregion

            var productDtoList = mapper.Map<List<ProductDto>>(productList.ToList());


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productDtoList.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTaxAsync(int id, PriceCalculator priceCalculator)
        {
            if(redisService.Database.KeyExists(productListCacheKey))
            {
                var productFromCacheAsJson = await redisService.Database.ListGetByIndexAsync(productListCacheKey, id > 0 ? id-1 : id);
                var productDtoFromCache = JsonSerializer.Deserialize<ProductDto>(productFromCacheAsJson.ToString());
                if (productDtoFromCache is not null)
                    return ResponseModelDto<ProductDto?>.Success(productDtoFromCache);
            }
            var hasProduct = await productRepositoryAsync.GetByIdAsync(id);

            if (hasProduct is null) return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            var productDto = mapper.Map<ProductDto>(hasProduct);

            return ResponseModelDto<ProductDto?>.Success(productDto);
        }

        public async Task<ResponseModelDto<IReadOnlyList<ProductDto>>> GetByPagingAsync(int page, int pageSize, PriceCalculator priceCalculator)
        {
            var productList = await productRepositoryAsync.GetByPagingAsync(page, pageSize);

            var productDtoList = mapper.Map<List<ProductDto>>(productList.ToList());

            return ResponseModelDto<IReadOnlyList<ProductDto>>.Success(productDtoList);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateAsync(int productId, ProductUpdateRequestDto request)
        { 
            redisService.Database.KeyDelete(productCacheKey);
            var hasProduct = await productRepositoryAsync.GetByIdAsync(productId);

            if (hasProduct is null)
                return ResponseModelDto<NoContent>.Fail("Güncellemeye çalıştığınız ürün bulunamadı!", HttpStatusCode.NotFound);

            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;
            hasProduct.Stock = request.Stock;

            await productRepositoryAsync.UpdateAsync(hasProduct);

            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductNameAsync(ProductNameUpdateRequestDto request)
        {
            await productRepositoryAsync.UpdateProductNameAsync(request.Name, request.Id);

            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
