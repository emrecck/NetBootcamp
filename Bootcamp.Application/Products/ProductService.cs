using Bootcamp.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Application.Products
{
    public class ProductService(IProductRepository productRepository, ICacheService cacheService)
    {
        public async Task<ResponseModelDto<int>> Create(ProductCreateRequestDto requset)
        {
            var product = new Product()
            {
                Name = requset.Name,
                 Price = requset.Price,
            };

            var result = await productRepository.Create(product);

            return ResponseModelDto<int>.Success(result.Id);
        }

        public async Task<ResponseModelDto<ProductDto>> GetById(int id)
        {
            var productFromCache = cacheService.Get<ProductDto>($"product:{id}");
            if (productFromCache is not null)
            {
                return ResponseModelDto<ProductDto>.Success(productFromCache);
            }

            var product = await productRepository.GetById(id);

            if(product is null )
                return ResponseModelDto<ProductDto>.Fail("Could not found any product with this id");

            cacheService.Add($"product:{id}", new ProductDto(product.Id, product.Name, product.Price));

            return ResponseModelDto<ProductDto>.Success(new ProductDto(product.Id, product.Name, product.Price));
        }
    }
}
