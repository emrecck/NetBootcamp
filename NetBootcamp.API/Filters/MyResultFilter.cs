using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBootcamp.API.Filters
{
    public class MyResultFilter : Attribute, IResultFilter
    {
        // Response
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        // Request
        public void OnResultExecuting(ResultExecutingContext context)
        {
            
        }
    }
}
