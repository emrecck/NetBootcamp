namespace NetBootcamp.API.Users.DTOs
{
    public record UserUpdateRequestDto(string Name, string Surname, string Email, string PhoneNumber);
}
