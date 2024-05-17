using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using NetBootcamp.API.Products.DTOs;
using NetBootcamp.API.Products.DTOs.ProductCreateUseCase;

namespace NetBootcamp.API.Products
{
    // Primary constructor
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        private readonly IProductService _productService = productService;

        // Method Injection using with "[FromServices]"
        [HttpGet]
        public IActionResult GetAll([FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.GetAllWithCalulatedTax(priceCalculator)); // Ok ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        // query string baseUrl/api/products?id=1
        // route        baseUrl/api/products/1

        [HttpGet("{productId:int}")]   // Parametreyi route dan alır. // Parametre tipi belirtilebilir route da. - ROUTE CONSTRAINT
        public IActionResult GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.GetByIdWithCalculatedTax(productId, priceCalculator));
            //if (!product.IsSuccess)
            //    return NotFound(); //404
            //return Ok(product); // ControllerBase sınıfından kalıtımla alınmış factory metot.
        }

        [HttpGet("page/{page:int}/pagesize/{pageSize:max(50)}")]    // route data types, max(): max kaç olabiliceğini belirtir integer olarak. Uymazsa 404 döner
        public IActionResult GetByPaging(int page, int pageSize, PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.GetByPaging(page, pageSize, priceCalculator));
        }

        //complex types => class, record, struct => request body as json
        //simple types => int, string, decimal => query string by default / route data

        [HttpPost]
        public IActionResult Create(ProductCreateRequestDto request)
        {
            var result = _productService.Create(request);
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
        public IActionResult Update([FromRoute] int productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(_productService.Update(productId, request));
        }

        [HttpPut("UpdateProductName")]
        public IActionResult UpdateProductName(ProductNameUpdateRequestDto request)
        {
            return CreateActionResult(_productService.UpdateProductName(request));
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
        public IActionResult Delete(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.Delete(productId, priceCalculator));
        }
    }
}

// DAKİKA 04:29 TE KALDIM.
