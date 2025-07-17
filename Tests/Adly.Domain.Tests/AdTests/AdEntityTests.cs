using Adly.Domain.Entities.Ad;
using FluentAssertions;

namespace Adly.Domain.Tests.AdTests;

public class AdEntityTests
{
    [Fact]
    public void Creating_Ads_With_Null_User_Should_Throw_Exception()
    {
        // Arrange
        var description = "Test Description";
        var name = "Test Ad";
        Guid? userId = null;
        Guid? categoryId = Guid.NewGuid();

        // Act 
        Action act = () => AdEntity.Create(name, description, userId, categoryId);

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("User Id Must Have A Value...");

    }

    [Fact]
    public void Creating_Ads_With_Null_Category_Should_Throw_Exception()
    {
        // Arrange
        var description = "Test Description";
        var name = "Test Ad";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = null;

        // Act 
        Action act = () => AdEntity.Create(name, description, userId, categoryId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Category Id Must Have A Value...");

    }

    [Fact]
    public void Creating_Ads_With_Empty_Category_Should_Throw_Exception()
    {
        // Arrange
        var description = "Test Description";
        var name = "Test Ad";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = Guid.Empty;

        // Act 
        Action act = () => AdEntity.Create(name, description, userId, categoryId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Category Id Must Have A Value...");

    }


    [Fact]
    public void Creating_Ads_With_Empty_User_Should_Throw_Exception()
    {
        // Arrange
        var description = "Test Description";
        var name = "Test Ad";
        Guid? userId = Guid.Empty;
        Guid? categoryId = Guid.NewGuid();

        // Act 
        Action act = () => AdEntity.Create(name, description, userId, categoryId);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User Id Must Have A Value...");

    }


    [Fact]
    public void Two_Categories_With_SameId_Must_Be_Equal()
    {
        // Arrange
        var description = "Test Description";
        var name = "Test Ad";
        Guid? userId = Guid.NewGuid();
        Guid? categoryId = Guid.NewGuid();
        Guid? id = Guid.NewGuid();


        // Act 
        var ad1 = AdEntity.Create(id, name, description, userId, categoryId);
        var ad2 = AdEntity.Create(id, name, description, userId, categoryId);

        ad1.Equals(ad2).Should().BeTrue();
    }

}