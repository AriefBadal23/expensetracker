using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTrackerTests;

public class UserTests:IClassFixture<TestDbFixture>
{
    private readonly TestDbFixture _fixture;
    
    public UserTests(TestDbFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task RegisterAsync_WithValidDto_CreatesDefaultUserBuckets()
    {
        // Arrange
        await using var db = _fixture.CreateContext();

        // Seed default buckets FIRST
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);

        var loggerMock = new Mock<ILogger<UserService>>();
        var configurationMock = new Mock<IConfiguration>();

        var storeMock = new Mock<IUserStore<ApplicationUser>>();

        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            storeMock.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var userService = new UserService(
            userManagerMock.Object,
            configurationMock.Object,
            loggerMock.Object,
            db);

        var dto = new RegisterUserDto
        {
            Email = "mary.doe@outlook.com",
            FirstName = "Mary",
            LastName = "Doe",
            Password = "Marvel01@"
        };

        // Act
        var result = await userService.RegisterAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);

        var userBuckets = await db.UserBuckets.ToListAsync();
        
        
        Assert.Equal(6, userBuckets.Count);
        
    }

}