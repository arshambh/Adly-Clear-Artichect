namespace Adly.Infrastructure.Identity.Models;

internal class JwtConfiguration
{
    public string SignInKey { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int ExpirationMinute { get; set; }
    public string EncryptionKey { get; set; } = default!;
}