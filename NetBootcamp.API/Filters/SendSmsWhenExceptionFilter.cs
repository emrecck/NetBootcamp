using Microsoft.AspNetCore.Mvc.Filters;

namespace NetBootcamp.API.Filters
{
    public class SendSmsWhenExceptionFilter : Attribute, IExceptionFilter   
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = false;   // true dersek hatayı ele almış varsayarız ve geriye ok status code döner.

            Console.WriteLine($"Hata! Sms gönderilemedi: {context.Exception.Message}");

            //TODO ResponseModelDto türünde bir exception dönmek gerekiyor. Her yerden aynı formatta response dönmeli
            
        }
    }
}
