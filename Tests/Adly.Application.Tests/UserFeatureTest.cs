using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adly.Application.Common;
using Adly.Application.Contracts.User;
using Adly.Application.Contracts.User.Models;
using Adly.Application.Extensions;
using Adly.Application.Feature.Common;
using Adly.Application.Feature.User.Commands.Register;
using Adly.Application.Feature.User.Queries.PasswordLogin;
using Adly.Application.Tests.Extensions;
using Adly.Domain.Entities.User;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NSubstitute;
using Xunit.Abstractions;

namespace Adly.Application.Tests
{
    public class UserFeatureTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITestOutputHelper _testOutputHelper;

        public UserFeatureTest(ITestOutputHelper testOutputHelper)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.RegisterApplicationValidator();
            _serviceProvider=serviceCollection.BuildServiceProvider();

            _testOutputHelper = testOutputHelper;
        }


        [Fact]
        public async Task Creating_New_User_Should_Be_Success()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");
            var registerUserRequest = new RegisterUserCommand(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Internet.UserName(),
                faker.Internet.Email(),
                faker.Phone.PhoneNumber(),
                password,
                password
            );


            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.PasswordCreateAsync(Arg.Any<UserEntity>(), registerUserRequest.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));


            // Act
            var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);
            var userRegisterResult = await userRegisterCommandHandler.Handle(registerUserRequest, default);


            // Assert
            userRegisterResult.IsSuccess.Should().BeTrue();
        }



        [Fact]
        public async Task User_Register_Email_Should_Be_Valid()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");
            var registerUserRequest = new RegisterUserCommand(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Internet.UserName(),
                string.Empty,
                faker.Phone.PhoneNumber(),
                password,
                password
            );


            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.PasswordCreateAsync(Arg.Any<UserEntity>(), registerUserRequest.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));


            // Act
            var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);

            var validationBehavior =
                new ValidateRequestBehavior<RegisterUserCommand, OperationResult<bool>>(_serviceProvider.GetRequiredService<IValidator<RegisterUserCommand>>());


            var userRegisterResult = await validationBehavior.Handle(registerUserRequest, default, userRegisterCommandHandler.Handle);


            // Assert
            userRegisterResult.IsSuccess.Should().BeFalse();

            _testOutputHelper.WritelineOperationResultErrors(userRegisterResult);
        }


        [Fact]
        public async Task Register_User_Password_And_Repeat_Password_Should_Be_Same()
        {
            // Arrenge
            var faker = new Faker();
            var registerUserRequest = new RegisterUserCommand(
                faker.Person.FirstName,
                faker.Person.LastName,
                faker.Internet.UserName(),
                faker.Internet.Email(),
                faker.Phone.PhoneNumber(),
                Guid.NewGuid().ToString("N"),
                Guid.NewGuid().ToString("N")
            );


            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.PasswordCreateAsync(Arg.Any<UserEntity>(), registerUserRequest.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));


            // Act
            var userRegisterCommandHandler = new RegisterUserCommandHandler(userManager);
            var validationBehavior =
                new ValidateRequestBehavior<RegisterUserCommand, OperationResult<bool>>(
                    _serviceProvider.GetRequiredService<IValidator<RegisterUserCommand>>());

            var userRegisterResult =
                await validationBehavior.Handle(registerUserRequest, default, userRegisterCommandHandler.Handle);


            // Assert
            userRegisterResult.IsSuccess.Should().BeFalse();

            _testOutputHelper.WritelineOperationResultErrors(userRegisterResult);
        }

        [Fact]
        public async Task Password_Login_User_With_UserName_Should_Be_Success()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");

            var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, password);

            var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
                faker.Person.Email);



            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.GetUserByUsernameAsync(loginQuery.UsernameOrEmail, CancellationToken.None)
                .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));

            var jwtService = NSubstitute.Substitute.For<IJwtService>();
            jwtService.GenerateTokenAsync(userEntity, default)
                .Returns(Task.FromResult<JwtAccessTokenModel>(new JwtAccessTokenModel("AccessToken",3000)));

            // Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            // Assert
            loginResult.Result.Should().NotBeNull();
            loginResult.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Password_Login_User_With_UserName_And_Wrong_Password_Should_Be_Failure()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");

            var loginQuery = new UserPasswordLoginQuery(faker.Person.UserName, password);

            var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
                faker.Person.Email);



            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.GetUserByUsernameAsync(loginQuery.UsernameOrEmail, CancellationToken.None)
                .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Failed()));

            var jwtService = NSubstitute.Substitute.For<IJwtService>();
            jwtService.GenerateTokenAsync(userEntity, default)
                .Returns(Task.FromResult<JwtAccessTokenModel>(new JwtAccessTokenModel("AccessToken", 3000)));

            // Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            // Assert
            loginResult.Result.Should().BeNull();
            loginResult.IsSuccess.Should().BeFalse();

            _testOutputHelper.WritelineOperationResultErrors(loginResult);

        }


        [Fact]
        public async Task Password_Login_User_With_Email_Should_Be_Success()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");

            var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, password);

            var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
                faker.Person.Email);



            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.GetUserByEmailAsync(loginQuery.UsernameOrEmail, CancellationToken.None)
                .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));

            var jwtService = NSubstitute.Substitute.For<IJwtService>();
            jwtService.GenerateTokenAsync(userEntity, default)
                .Returns(Task.FromResult<JwtAccessTokenModel>(new JwtAccessTokenModel("AccessToken", 3000)));

            // Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            // Assert
            loginResult.Result.Should().NotBeNull();
            loginResult.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Password_Login_User_Not_Found_Should_Be_Success()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");

            var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, password);

            var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
                faker.Person.Email);



            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.GetUserByEmailAsync(loginQuery.UsernameOrEmail, CancellationToken.None)
                .Returns(Task.FromResult<UserEntity?>(null));

            userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));

            var jwtService = NSubstitute.Substitute.For<IJwtService>();
            jwtService.GenerateTokenAsync(userEntity, default)
                .Returns(Task.FromResult<JwtAccessTokenModel>(new JwtAccessTokenModel("AccessToken", 3000)));

            // Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var loginResult = await userLoginQueryHandler.Handle(loginQuery, CancellationToken.None);

            // Assert
            loginResult.Result.Should().BeNull();
            loginResult.IsNotFound.Should().BeTrue();

            _testOutputHelper.WritelineOperationResultErrors(loginResult);

        }

        [Fact]
        public async Task Login_User_Inputs_Should_Be_Valid()
        {
            // Arrenge
            var faker = new Faker();
            var password = Guid.NewGuid().ToString("N");

            var loginQuery = new UserPasswordLoginQuery(faker.Person.Email, string.Empty);

            var userEntity = new UserEntity(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName,
                faker.Person.Email);



            var userManager = NSubstitute.Substitute.For<IUserManager>();
            userManager.GetUserByEmailAsync(loginQuery.UsernameOrEmail, CancellationToken.None)
                .Returns(Task.FromResult<UserEntity?>(userEntity));

            userManager.ValidatePasswordAsync(userEntity, loginQuery.Password, CancellationToken.None)
                .Returns(Task.FromResult(IdentityResult.Success));

            var jwtService = NSubstitute.Substitute.For<IJwtService>();
            jwtService.GenerateTokenAsync(userEntity, default)
                .Returns(Task.FromResult<JwtAccessTokenModel>(new JwtAccessTokenModel("AccessToken", 3000)));

            // Act
            var userLoginQueryHandler = new UserPasswordLoginQueryHandler(userManager, jwtService);

            var validationBehavior =
                new ValidateRequestBehavior<UserPasswordLoginQuery, OperationResult<JwtAccessTokenModel>>(
                    _serviceProvider.GetRequiredService<IValidator<UserPasswordLoginQuery>>());

            var loginResult = await validationBehavior.Handle(loginQuery, CancellationToken.None, userLoginQueryHandler.Handle);

            // Assert
            loginResult.Result.Should().BeNull();
            loginResult.IsSuccess.Should().BeFalse();

            _testOutputHelper.WritelineOperationResultErrors(loginResult);
        }

    }
}
