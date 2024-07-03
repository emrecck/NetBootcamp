using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.Service.Products;
using NetBootcamp.Service.Products.DTOs;
using NetBootcamp.Service.Products.Helpers;
using NetBootcamp.Service.Products.ProductCreateUseCase;

namespace NetBootcamp.API.Controllers
{
    // Primary constructor
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        private readonly IProductService _productService = productService;

        // Method Injection using with "[FromServices]"
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.GetAllWithCalulatedTax(priceCalculator)); // Ok ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        // query string baseUrl/api/products?id=1
        // route        baseUrl/api/products/1

        [HttpGet("{productId}")]   // Parametreyi route dan alır
        public async Task<IActionResult> GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.GetByIdWithCalculatedTax(productId, priceCalculator));
            //if (!product.IsSuccess)
            //    return NotFound(); //404
            //return Ok(product); // ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        //complex types => class, record, struct => request body as json
        //simple types => int, string, decimal => query string by default / route data

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequestDto request)
        {
            var result = await _productService.Create(request);
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
        public async Task<IActionResult> Update([FromRoute] int productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(await _productService.Update(productId, request));
        }

        /// <summary>
        /// PUT api/products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPut]
        //public IActionResult Update2(ProductUpdateRequestDto request)
        //{
        //    _productService.Update(request);
        //    return NoContent();
        //}

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.Delete(productId, priceCalculator));
        }
    }
}

// DAKİKA 04:29 TE KALDIM.
