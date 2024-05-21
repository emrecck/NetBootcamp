using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetBootcamp.API.DTOs;
using NetBootcamp.API.Products.Async;
using NetBootcamp.API.Products.DTOs;

namespace NetBootcamp.API.Filters
{
    public class NotFoundFilter(IProductRepositoryAsync productRepository) : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Buna actiondan response döneceğimiz için ihtiyaç yok
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            
            var firstParamFromAction = context.ActionArguments.Values.First()!;    // arguments methodun aldığı parametreler
            int productId = 0;

            if (actionName == "UpdateProductName" && firstParamFromAction is ProductNameUpdateRequestDto productNameUpdateRequestDto)
                productId = productNameUpdateRequestDto.Id;


            if (firstParamFromAction is not ProductNameUpdateRequestDto request && !int.TryParse(firstParamFromAction.ToString(), out productId))
                return;

            var hasProduct = productRepository.IsExist(productId).Result;  // method async olduğu için result ile data almayı bekledik.
            if (!hasProduct)
            {
                var errorMessage = $"There is no product with id: {productId}";
                var responseModel = ResponseModelDto<NoContent>.Fail(errorMessage);
                context.Result = new NotFoundObjectResult(responseModel);
            }
        }
    }
}
