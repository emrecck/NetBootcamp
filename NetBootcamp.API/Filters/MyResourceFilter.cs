using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBootcamp.API.Filters
{
    public class MyResourceFilter : Attribute, IResourceFilter
    {
        // Response
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }

        // Request 
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            
        }
    }
}
