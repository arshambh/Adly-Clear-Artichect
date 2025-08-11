using Adly.Application.Contracts.FileService.Interface;
using Adly.Application.Contracts.FileService.Models;
using Adly.Application.Contracts.User;
using Adly.Application.Extensions;
using Adly.Application.Feature.Ad.Commands;
using Adly.Application.Feature.Ad.Queries;
using Adly.Application.Repositories.Ad;
using Adly.Application.Repositories.Category;
using Adly.Application.Repositories.Common;
using Adly.Application.Repositories.Location;
using Adly.Domain.Common.ValueObjects;
using Adly.Domain.Entities.Ad;
using Adly.Domain.Entities.User;
using AutoMapper;
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

        serviceCollection
            .RegisterApplicationValidator()
            .AddApplicationAutoMapper()
            .AddLoggerFactory();

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

        var createAdResult = await Helpers.ValidateAndExecuteAsync(createAdCommand, createAdHandler, _serviceProvider);

        createAdResult.Result.Should().BeTrue();
    }






    [Fact]
    public async Task Editing_An_Ad_With_Valid_Parameters_Should_Be_Success()
    {

        var mockId = Guid.NewGuid();
        var adEntityMock = AdEntity.Create(mockId, "Test Title", "Test Description", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());


        var mockAdImages = new List<ImageValueObjects>()
        {
            new ImageValueObjects("TestFile1.png", "Image/png"),
            new ImageValueObjects("TestFile2.png", "Image/png"),
            new ImageValueObjects("TestFile3.png", "Image/png"),
        };
        mockAdImages.ForEach(x => adEntityMock.AddImage(x));

        var unitOfWorkMock = NSubstitute.Substitute.For<IUnitOfWork>();
        var adRepositoryMock = NSubstitute.Substitute.For<IAdRepository>();

        var categoryMock = NSubstitute.Substitute.For<ICategoryRepository>();
        var locationMock = NSubstitute.Substitute.For<ILocationRepository>();
        var fileServiceMock = NSubstitute.Substitute.For<IFileService>();



        categoryMock.GetCategoryByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(new CategoryEntity("Test Category")));


        locationMock.GetLocationByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(new LocationEntity("Test Location")));

        adRepositoryMock.GetAdByIdAsync(mockId, default)!.Returns(Task.FromResult(adEntityMock));


        fileServiceMock.SaveFilesAsync(Arg.Any<List<SaveFileModel>>())
            .Returns(Task.FromResult(new List<SaveFileModelResult>()
            {
                new ("Test.png", "image/png")
            }));

        fileServiceMock.RemoveFilesAsync(Arg.Any<List<string>>())
            .Returns(Task.CompletedTask);

        unitOfWorkMock.AdRepository.Returns(adRepositoryMock);
        unitOfWorkMock.LocationRepository.Returns(locationMock);
        unitOfWorkMock.CategoryRepository.Returns(categoryMock);



        var editAdCommand = new EditAdCommand(mockId, Guid.NewGuid(), Guid.NewGuid(),
            "Edited Title", "Edited Description", ["TestFile1.png"],
            [new EditAdCommand.AddNewImagesModel("Test Image Content", "image/png")]);


        var editAdCommandHandler = new EditAdCommandHandler(unitOfWorkMock, fileServiceMock);

        var editAdCommandResult = await Helpers.ValidateAndExecuteAsync(editAdCommand, editAdCommandHandler, _serviceProvider);


        //Assert
        editAdCommandResult.Result.Should().BeTrue();
        adEntityMock.Title.Should().BeEquivalentTo("Edited Title");
        adEntityMock.Description.Should().BeEquivalentTo("Edited Description"); // Corrected line
        adEntityMock.Images.Should().NotContain(x => x.FileName.Equals("TestFile1.png"));
        adEntityMock.Images.Should().HaveCount(3);
        adEntityMock.Images.Should().Contain(x => x.FileName.Equals("Test.png"));


    }


    [Fact]
    public async Task Getting_Add_Details_With_Valid_Parameters_Should_Success()
    {
        var userMock = new UserEntity("Test", "Test LastName", "TestUser", "Test@test.com")
        {
            PhoneNumber = "1234567"
        };

        var locationMock = new LocationEntity("Test Location");
        var categoryMock = new CategoryEntity("Test Category");
        var adMock = AdEntity.Create("Test Title", "Test Description", userMock, categoryMock, locationMock);


        var mockAdImages = new List<ImageValueObjects>()
        {
            new ImageValueObjects("TestFile1.png", "Image/png"),
            new ImageValueObjects("TestFile2.png", "Image/png"),
            new ImageValueObjects("TestFile3.png", "Image/png"),
        };
        mockAdImages.ForEach(x => adMock.AddImage(x));


        var unitOfWorkMock = NSubstitute.Substitute.For<IUnitOfWork>();
        var adRepositoryMock = NSubstitute.Substitute.For<IAdRepository>();
        var fileServiceMock = NSubstitute.Substitute.For<IFileService>();


        adRepositoryMock.GetAdDetailByIdAsync(adMock.Id)
            .Returns(Task.FromResult(adMock));

        fileServiceMock.GetFilesByNameAsync(Arg.Any<List<string>>())
            .Returns(Task.FromResult(new[]
            {
                new GetFileModel("TestFileUrl1", "image/png","TestFile1.png"),
                new GetFileModel("TestFileUrl2", "image/png","TestFile2.png"),
                new GetFileModel("TestFileUrl3", "image/png","TestFile3.png"),
            }));

        unitOfWorkMock.AdRepository.Returns(adRepositoryMock);

        var queryModel = new GetAdDetailByIdQuery(adMock.Id);
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        var queryHandler = new GetAdDetailByIdQueryHandler(unitOfWorkMock, fileServiceMock, mapper);

        var result = await Helpers.ValidateAndExecuteAsync(queryModel, queryHandler, _serviceProvider);

        result.IsSuccess.Should().BeTrue();

        result.Result!.CategoryName.Should().BeEquivalentTo(categoryMock.Name);
        result.Result!.OwnerUserName.Should().BeEquivalentTo(userMock.UserName);
        result.Result!.OwnerPhoneNumber.Should().BeEquivalentTo(userMock.PhoneNumber);

    }




}