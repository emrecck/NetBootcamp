namespace NetBootcamp.Services.Users;

public record SignUpRequestDto(string Name, string Surname, string Email, string Phone, string Username, string Password, DateTime BirthDate);