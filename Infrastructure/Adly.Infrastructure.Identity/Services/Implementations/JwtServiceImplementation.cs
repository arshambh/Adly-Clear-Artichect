using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adly.Application.Contracts.User;
using Adly.Application.Contracts.User.Models;
using Adly.Domain.Entities.User;
using Adly.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Adly.Infrastructure.Identity.Services.Implementations;

internal class JwtServiceImplementation(IUserClaimsPrincipalFactory<UserEntity> claimPrincipalFactory, IOptions<JwtConfiguration> jwtConfiguration) : IJwtService
{
    public async Task<JwtAccessTokenModel> GenerateTokenAsync(UserEntity user, CancellationToken cancellationToken)
    {
        var claims = await claimPrincipalFactory.CreateAsync(user);

        var secretKey = Encoding.UTF8.GetBytes(jwtConfiguration.Value.SignInKey);

        var encryptionKey = Encoding.UTF8.GetBytes(jwtConfiguration.Value.EncryptionKey);

        var signInCredential = new SigningCredentials(new SymmetricSecurityKey(secretKey)
            , SecurityAlgorithms.HmacSha256Signature);


        var encryptionCredential = new EncryptingCredentials(
            new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256
        );

        var descriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtConfiguration.Value.Issuer,
            Audience = jwtConfiguration.Value.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(0),
            Expires = DateTime.Now.AddMinutes(jwtConfiguration.Value.ExpirationMinute),
            SigningCredentials = signInCredential,
            Subject = new ClaimsIdentity(claims.Claims),
            EncryptingCredentials = encryptionCredential,
            TokenType = "JWE"
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(descriptor);


        return new JwtAccessTokenModel(tokenHandler.WriteToken(token), (token.ValidTo - DateTime.UtcNow).TotalSeconds);
    }


}