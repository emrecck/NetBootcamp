namespace NetBootcamp.Service.Users.DTOs
{
    public record UserDto(int Id, string Name, string Surname, string PhoneNumber, string Email,string CreatedDate);
}
