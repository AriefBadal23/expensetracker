using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Models;
using expensetrackerapi.Results;
using expensetrackerapi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTrackerTests;

public class BucketTests: IClassFixture<TestDbFixture>
{
    private readonly TestDbFixture _fixture;
    
    public BucketTests(TestDbFixture fixture)
    {
        _fixture = fixture;

    }
    
     [Fact]
    public async Task TestCorrectBucketSummaryJanuary2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();
        var bucketloggerMock = new Mock<ILogger<BucketService>>();
        
        
        var user = new RegisteredUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe"
        };
        
        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(user));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        
        var bucketService = new BucketService(db,bucketloggerMock.Object);
        
        
        // Act
        const int month = 1;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(seedingUser.Id,month, year);

        Assert.NotNull(summary.Value);
        var value = summary.Value;
        
        var summaryTotalIncome = summary.Value?.TotalIncome;
        var summaryTotalExpenses = summary.Value?.TotalExpenses;
        // Assert
        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(385, summaryTotalExpenses);
        Assert.Equal(3,value.Buckets.Count);
        Assert.Contains(nameof(Buckets.Salary), value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Groceries), value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Shopping), value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,value.Month);
        Assert.Equal(year,value.Year);
        
    }
    [Fact]
    public async Task TestCorrectBucketSummaryMarch2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();
        var userServiceMock = new Mock<IUserService>();

        var user = new RegisteredUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe"
        };
        
        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(user));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        
        // Act
        const int month = 3;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(seedingUser.Id, month, year);
        
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;

        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(398, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(nameof(Buckets.Salary), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Groceries), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Shopping), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    [Fact]
    public async Task TestCorrectBucketSummaryAugust2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();

        var user = new RegisteredUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe"
        };

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(user));


        var seeder = new DbIntializer();
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        // Act
        const int month = 8;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(seedingUser.Id,month, year);
        
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(108, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(nameof(Buckets.Salary), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Groceries), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Shopping), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    [Fact]
    public async Task TestInCorrectBucketSummaryJanuary2026()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();

        var user = new RegisteredUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe"
        };
        
        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(user));

        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        
        // Act
        const int month = 1;
        const int year = 2026;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(seedingUser.Id,month, year);
        
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;
        

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(0, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(nameof(Buckets.Salary), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Groceries), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(nameof(Buckets.Shopping), summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    
    [Fact]
    public async Task CreateBucket_WithValidDto_CreatesNewUserBucket()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        
        var userServiceMock = new Mock<IUserService>();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();

        var user = new RegisteredUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe"
        };
        
        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            // Returns what the service should return as a result to mock it.
            .ReturnsAsync(Result<RegisteredUserDto>.Success(user)); 

        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        
        // Act
        BucketRequestDto newUserBucket = new BucketRequestDto
        {
            Icon = "💸",
            Name = "Savings",
            Type = BucketTypes.Expense
        };
        
        await bucketService.CreateBucket(seedingUser.Id, newUserBucket );
        
        var newCreatedBucket =  await db.Buckets.Where(ub => newUserBucket.Name == ub.Name).FirstAsync();
        
        var newCreatedUserBucket =  await db.UserBuckets.Where(ub =>
            ub.ApplicationUserId == seedingUser.Id && ub.BucketId == newCreatedBucket.Id).Select(x => x).FirstAsync();
        
        //Assert
        Assert.Equal("Savings", newCreatedBucket.Name);
        Assert.Equal("💸", newCreatedBucket.Icon);
        Assert.Equal(0, newCreatedUserBucket.Total);
    }

    [Fact]
    public async Task DeleteBucket_WithValidBucketId_DeletesBucketSuccesfully()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();
        
        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        
        var newBucket = new Bucket
        {
            Icon = "🚗",
            Name = "Car insurence",
            Type = BucketTypes.Expense
        };
        await db.Buckets.AddAsync(newBucket);
        await db.SaveChangesAsync();

        var newCreatedBucket = await db.Buckets.FirstAsync(b => b.Name == newBucket.Name);
        
        var newUserBucket = new UserBuckets
        {
            ApplicationUserId = seedingUser.Id,
            BucketId = newCreatedBucket.Id,
            Total = 0
        };
        db.UserBuckets.Add(newUserBucket);
        
        await db.SaveChangesAsync();
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        // Act
        var bucketIsDeleted = await bucketService.DeleteBucket(seedingUser.Id, newUserBucket.BucketId);

        // Assert
        Assert.True(bucketIsDeleted.Value);
    }
    
    [Fact]
    public async Task DeleteBucket_WithInValidBucketId_DeletesBucketFailed()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();
        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        // Act
        // Delete a default bucket (Salary, Groceries and Shopping) which is not allowed.
        var salarybucketIsDeleted = await bucketService.DeleteBucket(seedingUser.Id, 1);
        var shoppingbucketIsDeleted = await bucketService.DeleteBucket(seedingUser.Id, 2);
        var groceriesbucketIsDeleted = await bucketService.DeleteBucket(seedingUser.Id, 3);

        // Assert
        Assert.False(salarybucketIsDeleted.Value);
        Assert.False(shoppingbucketIsDeleted.Value);
        Assert.False(groceriesbucketIsDeleted.Value);
    }
    
    [Fact]
    public async Task DeleteBucket_NotExisting_BucketId_DeletesBucketFailed()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        
        var bucketloggerMock = new Mock<ILogger<BucketService>>();
        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        // Act
        // Delete a default bucket (Salary, Groceries and Shopping) which is not allowed.
        var notExistingBucket = await bucketService.DeleteBucket(seedingUser.Id, 100);

        // Assert
        Assert.False(notExistingBucket.Value);
    }

    [Fact]
    public async Task DeleteBucket_OtherUserId_DeletesBucketFailed()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var bucketloggerMock = new Mock<ILogger<BucketService>>();

       
        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var seedingUser = await db.Users.FirstAsync(u => u.UserName == "john.doe@outlook.nl");
        
        var otherExistingUser = await db.Users.FirstAsync(u => u.UserName == "arief@outlook.nl");
        
        // create a new bucket for the other user we are trying to get the data from that we should not see.
        var newBucket = new Bucket
        {
            Icon = "🚗",
            Name = "Car insurence",
            Type = BucketTypes.Expense
        };
        await db.Buckets.AddAsync(newBucket);
        await db.SaveChangesAsync();

        var newCreatedBucket = await db.Buckets.FirstAsync(b => b.Name == newBucket.Name);
        
        var newUserBucket = new UserBuckets
        {
            ApplicationUserId = otherExistingUser.Id,
            BucketId = newCreatedBucket.Id,
            Total = 0
        };
        db.UserBuckets.Add(newUserBucket);
        
        var bucketService = new BucketService(db, bucketloggerMock.Object);
        
        // Act
        var canSeeOtherUserBucketData = await bucketService.DeleteBucket(seedingUser.Id, newCreatedBucket.Id);

        // Assert
        Assert.False(canSeeOtherUserBucketData.Value);
    }
    
    
}