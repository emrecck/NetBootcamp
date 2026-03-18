namespace NetBootcamp.Services.Token.Dtos;

public record TokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpireAt);