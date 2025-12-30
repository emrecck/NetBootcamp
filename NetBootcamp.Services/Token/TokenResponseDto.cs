namespace NetBootcamp.Services.Token
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpireAt { get; set; } = default!;
    }
}
