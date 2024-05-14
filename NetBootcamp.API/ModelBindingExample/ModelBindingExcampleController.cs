using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using NetBootcamp.API.ModelBindingExample;

namespace NetBootcamp.API.NewFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelBindingExcampleController : ControllerBase
    {
        // [FromQuery] = simple => default
        // [FromBody] = complex => default
        // [FromHeader]
        // [FromRoute]



        // [FromQuery] = localhost:7063/api/ModelBindingExcample/User.Id=5&user.Name=Emre&user.Email=test@gmail.com
        // [HttpPost]
        //  public IActionResult SaveProduct([FromQuery] UserDto user)
        //  {
        //      return Ok(user);
        //  }

        // Parametrelere FromQuery yazmaya gerek yoktur çünkü varsayılan olarak FromQuery vardır.

        //  FromHeader simple type
        //  [HttpGet]
        //  public IActionResult SaveProduct(string name, string email)
        //  {
        //      return Ok(new { Name = name, Email = email });
        //  }

        //  FromHeader simple type
        //  [HttpGet]
        //  public IActionResult SaveProduct([FromHeader] string name, [FromHeader] string email)
        //  {
        //      return Ok(new { Name = name, Email = email });
        //  }

        // FromHeader complex type --> Modelin propertylerine de tek tek From Header eklemek gerekir.
        // Propertylerin nereden geleceğini ayrı ayrı belirleyebiliriz. kimisine FromHeader kimisine FromBody ekleyerek farklı yerlerden data alabiliriz.
        //  [HttpPost]
        //  public IActionResult SaveProduct([FromHeader] UserDto user)
        //  {
        //      return Ok(user);
        //  }

        // FromBody simple type
        //[HttpPost]
        //public IActionResult SaveProduct([ FromBody] string name)
        //{
        //    return Ok(new { Name = name });
        //}

        // FromRoute
        // localhost/api/modelbindingexample/name/emre/email/emrecck@gmail.com
        // localhost/api/modelbindingexample/emre/emrecck@gmail.com
        //[HttpGet("{name}/{email}")]
        //public IActionResult SaveProduct([FromRoute] string name, [FromRoute] string email)
        //{
        //    return Ok(new { Name = name, Email = email });
        //}

        //[HttpGet("name/{name}/email/{email}")]
        //public IActionResult SaveProduct(string name, string email)
        //{
        //    return Ok(new { Name = name, Email = email });
        //}
    }
}
