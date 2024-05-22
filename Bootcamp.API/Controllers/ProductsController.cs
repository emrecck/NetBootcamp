using Bootcamp.Application.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductService productService): ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateRequestDto request)
        {
            var response = await productService.Create(request);
            if (response.IsSuccess)
            {
                return Created(string.Empty, response);
            }

            return BadRequest(response.FailMessages);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await productService.GetById(id);
            if (response.IsSuccess)
            {
                 return Ok(response);
            }

            return BadRequest(response.FailMessages);
        }
    }
}
