namespace ElevenNote.Models.Token
{
    public class TokenResponse
    {
        public string Token { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
        public DateTime Expires { get; set; }
    }
}