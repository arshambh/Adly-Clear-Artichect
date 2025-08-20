using System.Runtime.CompilerServices;
using Adly.Application.Common;
using Adly.Application.Extensions;
using Adly.Application.Feature.Common;
using Adly.Application.Feature.Location.Commands;
using Adly.Application.Feature.Location.Queries;
using Adly.Application.Feature.User.Commands.Register;
using Adly.Application.Repositories.Common;
using Adly.Application.Repositories.Location;
using Adly.Application.Tests.Extensions;
using Adly.Domain.Entities.Ad;
using Bogus.DataSets;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Adly.Application.Tests;

public class LocationFeaturesTests
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public LocationFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.RegisterApplicationValidator();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task Add_Location_With_Valid_Parameters_Should_Success()
    {
        // Arrange
        var faker = new Bogus.Faker();
        var location = new CreateLocationCommand(faker.Address.City());

        var locationRepositoryMock = NSubstitute.Substitute.For<ILocationRepository>();
        locationRepositoryMock.IsLocationExistsAsync(location.LocationName).Returns(Task.FromResult(false));

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        unitOfWork.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior =
            new ValidateRequestBehavior<CreateLocationCommand, OperationResult<bool>>(_serviceProvider.GetRequiredService<IValidator<CreateLocationCommand>>());

        var createLocationHandler =
            new CreateLocationCommandHandler(unitOfWork);

        // Act
        var createLocationResult = await validationBehavior.Handle(
            location,
            createLocationHandler.Handle,
            CancellationToken.None);

        // Assert

        createLocationResult.Result.Should().BeTrue();
    }



    [Fact]
    public async Task Existing_Location_Cannot_BeCreated()
    {
        // Arrange
        var faker = new Bogus.Faker();
        var location = new CreateLocationCommand(faker.Address.City());

        var locationRepositoryMock = NSubstitute.Substitute.For<ILocationRepository>();
        locationRepositoryMock.IsLocationExistsAsync(location.LocationName).Returns(Task.FromResult(true));

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        unitOfWork.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior =
            new ValidateRequestBehavior<CreateLocationCommand, OperationResult<bool>>(_serviceProvider.GetRequiredService<IValidator<CreateLocationCommand>>());

        var createLocationHandler =
            new CreateLocationCommandHandler(unitOfWork);

        // Act
        var createLocationResult = await validationBehavior.Handle(
            location,
            createLocationHandler.Handle,
            CancellationToken.None);

        // Assert
        createLocationResult.Result.Should().BeFalse();


        _testOutputHelper.WritelineOperationResultErrors(createLocationResult);
    }



    [Fact]
    public async Task Getting_List_Of_Location_Should_Be_Success()
    {
        // Arrange
        var faker = new Bogus.Faker();
        var location = new GetLocationByNameQuery(faker.Address.City());
        List<LocationEntity> fakeLocations = [new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City())
        ];


        var locationRepositoryMock = NSubstitute.Substitute.For<ILocationRepository>();
        locationRepositoryMock.GetLocationByNameAsync(location.LocationNameSearchTerm).Returns(Task.FromResult(fakeLocations));

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        unitOfWork.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior =
            new ValidateRequestBehavior<GetLocationByNameQuery, OperationResult<List<GetLocationByNameQueryResult>>>(_serviceProvider.GetRequiredService<IValidator<GetLocationByNameQuery>>());

        var getLocationByNameQueryHandler =
            new GetLocationByNameQueryHandler(unitOfWork);

        // Act
        var getLocationResult = await validationBehavior.Handle(
            location,
            getLocationByNameQueryHandler.Handle,
            CancellationToken.None);

        // Assert
        getLocationResult.Result.Should().NotBeEmpty();


    }



    [Fact]
    public async Task Searching_For_Location_Should_Have_At_Least_Three_Characters()
    {
        // Arrange
        var faker = new Bogus.Faker();
        var location = new GetLocationByNameQuery(faker.Address.City()[..2]);
        List<LocationEntity> fakeLocations = [new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City()),
            new LocationEntity(faker.Address.City())
        ];


        var locationRepositoryMock = NSubstitute.Substitute.For<ILocationRepository>();
        locationRepositoryMock.GetLocationByNameAsync(location.LocationNameSearchTerm).Returns(Task.FromResult(fakeLocations));

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        unitOfWork.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior =
            new ValidateRequestBehavior<GetLocationByNameQuery, OperationResult<List<GetLocationByNameQueryResult>>>(_serviceProvider.GetRequiredService<IValidator<GetLocationByNameQuery>>());

        var getLocationByNameQueryHandler =
            new GetLocationByNameQueryHandler(unitOfWork);

        // Act
        var getLocationResult = await validationBehavior.Handle(
            location,
            getLocationByNameQueryHandler.Handle,
            CancellationToken.None);

        // Assert
        getLocationResult.IsSuccess.Should().BeFalse();

        _testOutputHelper.WritelineOperationResultErrors(getLocationResult);
    }



 

    [Fact]
    public async Task Location_Name_Should_Not_Be_Empty()
    {
        // Arrange
        var emptyLocationName = string.Empty;
        var location = new CreateLocationCommand(emptyLocationName);

        var locationRepositoryMock = NSubstitute.Substitute.For<ILocationRepository>();
        locationRepositoryMock.IsLocationExistsAsync(location.LocationName).Returns(Task.FromResult(false));

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        unitOfWork.LocationRepository.Returns(locationRepositoryMock);

        var validationBehavior =
            new ValidateRequestBehavior<CreateLocationCommand, OperationResult<bool>>(
                _serviceProvider.GetRequiredService<IValidator<CreateLocationCommand>>());

        var createLocationHandler =
            new CreateLocationCommandHandler(unitOfWork);

        // Act
        var createLocationResult = await validationBehavior.Handle(
            location,
            createLocationHandler.Handle,
            CancellationToken.None);

        // Assert
        createLocationResult.IsSuccess.Should().BeFalse();
        _testOutputHelper.WritelineOperationResultErrors(createLocationResult);
    }



}