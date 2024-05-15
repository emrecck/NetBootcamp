namespace NetBootcamp.API.Users.DTOs
{
    public record UserCreateRequestDto(string Name, string Surname, string PhoneNumber, string Email);
}
