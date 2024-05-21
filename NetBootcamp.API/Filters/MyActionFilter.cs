using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBootcamp.API.Filters
{
    public class MyActionFilter : Attribute, IActionFilter
    {
        // Response
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        //Request
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
