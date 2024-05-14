using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json.Serialization;

namespace NetBootcamp.API.DTOs
{
    public struct NoContent; // Structlar value type lardır.

    public record ResponseModelDto<T>
    {
        //List<Product> Data --> Object Boxing
        // Object --> List<Product> Unboxing
        public T? Data { get; init; }

        /// <summary>
        /// Json Ignore Response dönerken IsSuccess propertysinin serialize edilmesini engeller.
        /// </summary>
        [JsonIgnore] public bool IsSuccess { get; init; }
        public List<string>? FailMessages { get; init; }
        [JsonIgnore] public HttpStatusCode StatusCodes { get; set; }

        // static factory methods
        public static ResponseModelDto<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModelDto<T>()
            {
                Data = data,
                IsSuccess = true,
                StatusCodes = statusCode
            };
        }

        public static ResponseModelDto<T> Success(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = true,
                StatusCodes = statusCode
            };
        }

        public static ResponseModelDto<T> Fail(List<string> failMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = false,
                FailMessages = failMessages,
                StatusCodes = statusCode
            };
        }

        public static ResponseModelDto<T> Fail(string failMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = false,
                FailMessages = new List<string> { failMessage },
                StatusCodes = statusCode
            };
        }
    }
}
