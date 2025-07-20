namespace Adly.Application.Contracts.User.Models;

public record JwtAccessTokenModel(string AccessToken,int ExpireInSeconds, string TokenType="Bearer");