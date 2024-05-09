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
        [JsonIgnore]
        public bool IsSuccess { get; init; }
        public List<string>? FailMessages { get; init; }

        // static factory methods
        public static ResponseModelDto<T> Success(T data)
        {
            return new ResponseModelDto<T>()
            {
                Data = data,
                IsSuccess = true,
                FailMessages = null
            };
        }

        public static ResponseModelDto<T> Success()
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = true
            };
        }

        public static ResponseModelDto<T> Fail(List<string> failMessages)
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = false,
                FailMessages = failMessages
            };
        }

        public static ResponseModelDto<T> Fail(string failMessage)
        {
            return new ResponseModelDto<T>()
            {
                IsSuccess = false,
                FailMessages = new List<string> { failMessage }
            };
        }
    }
}
