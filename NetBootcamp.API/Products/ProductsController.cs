using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.API.Filters;
using NetBootcamp.Services.Products.Asyncs;
using NetBootcamp.Services.Products.DTOs;
using NetBootcamp.Services.Products.Helpers;
using NetBootcamp.Services.Products.ProductCreateUseCase;

namespace NetBootcamp.API.Products
{
    // Primary constructor
    public class ProductsController(IProductServiceAsync productServiceAsync) : CustomBaseController
    {
        private readonly IProductServiceAsync _productServiceAsync = productServiceAsync;

        // Method Injection using with "[FromServices]"
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] PriceCalculator priceCalculator)
        {
            var result = await _productServiceAsync.GetAllWithCalulatedTaxAsync(priceCalculator);
            return CreateActionResult(result); // Ok ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        // query string baseUrl/api/products?id=1
        // route        baseUrl/api/products/1

        [HttpGet("{productId:int:min(0)}")]   // Parametreyi route dan alır. // Parametre tipi belirtilebilir route da. - ROUTE CONSTRAINT
        [MyResourceFilter]
        [MyActionFilter]
        [MyResultFilter]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productServiceAsync.GetByIdWithCalculatedTaxAsync(productId, priceCalculator));
            //if (!product.IsSuccess)
            //    return NotFound(); //404
            //return Ok(product); // ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        [HttpGet("page/{page:int}/pagesize/{pageSize:max(50)}")]    // route data types, max(): max kaç olabiliceğini belirtir integer olarak. Uymazsa 404 döner
        public async Task<IActionResult> GetByPaging(int page, int pageSize, PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productServiceAsync.GetByPagingAsync(page, pageSize, priceCalculator));
        }
         
        //complex types => class, record, struct => request body as json
        //simple types => int, string, decimal => query string by default / route data

        [HttpPost]
        [SendSmsWhenExceptionFilter]
        public async Task<IActionResult> Create(ProductCreateRequestDto request)
        {
            //throw new Exception("db ye gidemedi");

            var result = await _productServiceAsync.CreateAsync(request);
            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });

            //if (!result.IsSuccess)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, result.FailMessages);
            //}

            //return CreatedAtAction(nameof(GetById), new {productId = result.Data}, null); // returns 201 Created --> Response modelin header ında kaydedilen ürüne erişmek için bir url döndürür.
        }

        /// <summary>
        /// PUT api/products/10
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> Update([FromRoute] int productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(await _productServiceAsync.UpdateAsync(productId, request));
        }

        [HttpPut("UpdateProductName")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> UpdateProductName(ProductNameUpdateRequestDto request)
        {
            return CreateActionResult(await _productServiceAsync.UpdateProductNameAsync(request));
        }

        /// <summary>
        /// PUT api/products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPut]
        //public IActionResult Update2(ProductUpdateRequestDto request)
        //{
        //    _productServiceAsync.Update(request);
        //    return NoContent();
        //}

        [HttpDelete("{productId}")]
        [ServiceFilter(typeof(NotFoundFilter))] // NotFoundFilter constructor ında injection aldığı için ServiceFilter ile wrapliyoruz.
        public async Task<IActionResult> Delete(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productServiceAsync.DeleteAsync(productId, priceCalculator));
        }
    }
}

// DAKİKA 04:29 TE KALDIM.
