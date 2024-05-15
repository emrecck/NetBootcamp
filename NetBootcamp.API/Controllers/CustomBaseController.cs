using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.DTOs;
using System.Net;

namespace NetBootcamp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        /// <summary>
        /// This method has been created for Create Actions.
        /// Becasuse of that when a create request taken, then this method provide a link for created item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="methodName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public IActionResult CreateActionResult<T>(ResponseModelDto<T> response, string methodName, object? routeValues)
        {
            if (response.StatusCodes == HttpStatusCode.NoContent)
                return CreatedAtAction(methodName, routeValues, null);
            
            return new ObjectResult(response) { StatusCode = (int)response.StatusCodes };
        }

        public IActionResult CreateActionResult<T>(ResponseModelDto<T> response)
        {
            if (response.StatusCodes == HttpStatusCode.NoContent)
                return new ObjectResult(null) { StatusCode = 204 };

            return new ObjectResult(response) { StatusCode = (int)response.StatusCodes };
        }
    }
}
