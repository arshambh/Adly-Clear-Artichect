using Adly.Application.Feature.User.Commands.Register;
using Adly.Application.Feature.User.Queries.PasswordLogin;
using Adly.Domain.Entities.User;
using FluentAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Adly.Identity.Tests;

public class IdentityTests(IdentityTestSetup setup) : IClassFixture<IdentityTestSetup>
{
    private readonly IServiceProvider _serviceProvider = setup.ServiceProvider;

    [Fact]
    public async Task Register_User_Should_Success()
    {
        var testUser = new RegisterUserCommand("Test", "Test", "test", "test@test.com"
            , "09121231212", "qw123321", "qw123321");



        var sender = _serviceProvider.GetRequiredService<ISender>();
        var registerResult = await sender.Send(testUser);
        registerResult.IsSuccess.Should().BeTrue();





    }


    [Fact]
    public async Task Getting_Access_Token_Should_Success()
    {
        var testUser = new RegisterUserCommand("Test", "Test", "test2", "test2@test.com"
            , "09121231213", "qw123321", "qw123321");

        var sender = _serviceProvider.GetRequiredService<ISender>();

        await sender.Send(testUser);

        var tokenQuery = new UserPasswordLoginQuery("test2", "qw123321");

        var tokenQueryResult = await sender.Send(tokenQuery);

        tokenQueryResult.Result!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Jwe_Token_Should_Have_Claims()
    {
        var testUser = new RegisterUserCommand("Test", "Test", "test2", "test2@test.com"
            , "09121231213", "qw123321", "qw123321");

        var sender = _serviceProvider.GetRequiredService<ISender>();

        await sender.Send(testUser);

        var tokenQuery = new UserPasswordLoginQuery("test2", "qw123321");

        var tokenQueryResult = await sender.Send(tokenQuery);

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = "ShouldBe-LongerThan-16Char-SecretKey"u8.ToArray();

        var encryptionkey = "16CharEncryptKey"u8.ToArray();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
        };

        var claimPrincipals = tokenHandler.ValidateToken(tokenQueryResult.Result.AccessToken, tokenValidationParameters,
            out _);

        claimPrincipals.Claims.Should().NotBeEmpty();

    }

}