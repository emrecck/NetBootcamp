using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.Helpers;
using NetBootcamp.API.Products.ProductCreateUseCase;
using NetBootcamp.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace NetBootcamp.API.Products.Async
{
    // Primary constructor
    public class ProductServiceAsync(IProductRepositoryAsync productRepositoryAsync, IUnitOfWork unitOfWork, IMapper mapper) : IProductServiceAsync
    {
        public async Task<ResponseModelDto<int>> CreateAsync(ProductCreateRequestDto request)
        {
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
            var productList = await productRepositoryAsync.GetAllAsync();

            var productDtoList = mapper.Map<List<ProductDto>>(productList.ToList());

           // var productDtoList = productList.Select(product => new ProductDto
           //(product.Id, product.Name, priceCalculator.CalculateKdv(product.Price, 1.2m), product.Created.ToShortTimeString(), product.Barcode, product.Stock)).ToImmutableList(); // immutable list geri yeni bir referans döner

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productDtoList.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTaxAsync(int id, PriceCalculator priceCalculator)
        {
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
