using Microsoft.AspNetCore.Mvc;

namespace NetBootcamp.API.ModelBindingExample
{
    public record UserDto(int Id, string Name, string Email);
}
