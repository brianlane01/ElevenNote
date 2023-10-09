using ElevenNote.Models.Token;

namespace ElevenNote.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponse?> GetTokenAsync(TokenRequest model);
    }
}