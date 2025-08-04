using Adly.Application.Extensions;
using Adly.Application.Feature.Category.Queries;
using Adly.Application.Repositories.Category;
using Adly.Application.Repositories.Common;
using Adly.Application.Tests.Extensions;
using Adly.Domain.Entities.Ad;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit.Abstractions;

namespace Adly.Application.Tests;

public class CategoryFeaturesTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITestOutputHelper _testOutputHelper;

    public CategoryFeaturesTests(ITestOutputHelper testOutputHelper)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.RegisterApplicationValidator();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Get_Category_By_Id_Should_Return_Success()
    {
        // Arrange
        var getCategoryById = new GetCategoryByIdQuery(Guid.NewGuid());

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        var categoryRepository = NSubstitute.Substitute.For<ICategoryRepository>();

        categoryRepository.GetCategoryByIdAsync(getCategoryById.CategoryId)!
            .Returns(Task.FromResult(new CategoryEntity("Test")));

        unitOfWork.CategoryRepository.Returns(categoryRepository);

        var handler = new GetCategoryByIdQueryHandler(unitOfWork);

        // Act
        var result = await Helpers.ValidateAndExecuteAsync(getCategoryById, handler, _serviceProvider);

        // Assert
        result.Result.Should().NotBeNull();

    }


    [Fact]
    public async Task Get_Category_By_Empty_Id_Should_Return_Not_Success()
    {
        // Arrange
        var getCategoryById = new GetCategoryByIdQuery(Guid.Empty);

        var unitOfWork = NSubstitute.Substitute.For<IUnitOfWork>();
        var categoryRepository = NSubstitute.Substitute.For<ICategoryRepository>();

        categoryRepository.GetCategoryByIdAsync(getCategoryById.CategoryId)!
            .Returns(Task.FromResult(new CategoryEntity("Test")));

        unitOfWork.CategoryRepository.Returns(categoryRepository);

        var handler = new GetCategoryByIdQueryHandler(unitOfWork);

        // Act
        var result = await Helpers.ValidateAndExecuteAsync(getCategoryById, handler, _serviceProvider);

        // Assert
        result.Result.Should().BeNull();
        _testOutputHelper.WritelineOperationResultErrors(result);

    }

}