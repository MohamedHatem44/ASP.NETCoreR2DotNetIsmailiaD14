namespace ASP.NETCoreD14.DTOs.Auth
{
    public record TokenDto(string Token, int DurationInMinutes, string TokenType = "Bearer");
}
