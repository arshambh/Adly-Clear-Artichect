using Adly.Application.Contracts.FileService.Interface;
using Adly.Application.Contracts.FileService.Models;
using Adly.Application.Contracts.User;
using Adly.Application.Extensions;
using Adly.Application.Feature.Ad.Commands;
using Adly.Application.Repositories.Ad;
using Adly.Application.Repositories.Category;
using Adly.Application.Repositories.Common;
using Adly.Application.Repositories.Location;
using Adly.Domain.Entities.Ad;
using Adly.Domain.Entities.User;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Adly.Application.Tests;

public class AdFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public AdFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.RegisterApplicationValidator();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Create_Ad_With_Valid_Parameters_Should_Success()
    {
        // Arrange

        var createAdCommand = new CreateAdCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Test Title",
            "Test Description",
            [new CreateAdCommand.CreateAdImagesModel("Test", "image/png")]);

        var unitOfWorkMock = NSubstitute.Substitute.For<IUnitOfWork>();
        var adRepositoryMock = NSubstitute.Substitute.For<IAdRepository>();
        var userManagerMock = NSubstitute.Substitute.For<IUserManager>();
        var categoryMock = NSubstitute.Substitute.For<ICategoryRepository>();
        var locationMock = NSubstitute.Substitute.For<ILocationRepository>();
        var fileServiceMock = NSubstitute.Substitute.For<IFileService>();

        adRepositoryMock.CreateAdAsync(Arg.Any<AdEntity>()).Returns(Task.CompletedTask);

        categoryMock.GetCategoryByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(new CategoryEntity("Test Category")));


        locationMock.GetLocationByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(new LocationEntity("Test Location")));

        userManagerMock.GetUserByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(new UserEntity("Test", "Test", "Test", "Test@Test.com")));


        fileServiceMock.SaveFilesAsync(Arg.Any<List<SaveFileModel>>())
            .Returns(Task.FromResult(new List<SaveFileModelResult>()
        {
            new ("Test.png", "image/png")
        }));


        unitOfWorkMock.AdRepository.Returns(adRepositoryMock);
        unitOfWorkMock.LocationRepository.Returns(locationMock);
        unitOfWorkMock.CategoryRepository.Returns(categoryMock);


        var createAdHandler = new CreateAdCommandHandler(unitOfWorkMock, fileServiceMock, userManagerMock);

        var createAdResult=await Helpers.ValidateAndExecuteAsync(createAdCommand, createAdHandler, _serviceProvider);

        createAdResult.Result.Should().BeTrue();
    }







}