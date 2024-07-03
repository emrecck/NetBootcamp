namespace NetBootcamp.Service.Users.UserCreateUseCase
{
    public record UserCreateRequestDto(string Name, string Surname, string PhoneNumber, string Email);
}
