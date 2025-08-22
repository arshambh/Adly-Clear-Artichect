namespace Adly.Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken,double ExpireInSeconds, string TokenType="Bearer");