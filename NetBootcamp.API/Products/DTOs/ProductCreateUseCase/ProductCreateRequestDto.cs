using System.ComponentModel.DataAnnotations;

namespace NetBootcamp.API.Products.DTOs.ProductCreateUseCase
{
    public record ProductCreateRequestDto(string Name, decimal Price); // Data record lar olarak adlandırılır. with C# 12

    //public record ProductCreateRequestDto(
    //[Required(ErrorMessage = "Product name is required.")]
    //[StringLength(maximumLength:10, ErrorMessage = "Product name must be 10 characters.")]
    //string Name,
    //[Range(1, Int32.MaxValue)]
    //decimal Price); // Data record lar olarak adlandırılır. with C# 12
}
