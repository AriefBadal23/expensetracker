using System.ComponentModel.DataAnnotations;
using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Results;
using expensetrackerapi.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTrackerTests;

using expensetrackerapi;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.EntityFrameworkCore;
using NodaTime;


/* 
⚠️ Fixture gebruiken om 1 malig een in-memory database te maken en te gebruiken

 */

public class TestDbFixture
{
    public ExpenseTrackerContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ExpenseTrackerContext(options);
    }
}

public class ExpenseTrackerTests : IClassFixture<TestDbFixture>
{

    private readonly TestDbFixture _fixture;

    public ExpenseTrackerTests(TestDbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestCreateTransaction()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        

        // Act

        var buckets = new[]
        {
                new Bucket{Id=1, Name=Buckets.Salary,Icon="💰"},
                new Bucket{Id=2, Name=Buckets.Groceries,Icon="🏪"},
                new Bucket{Id=3, Name=Buckets.Shopping,Icon="🛒"}
        };

        db.Buckets.AddRange(buckets);
        await db.SaveChangesAsync();
        var loggerMock = new Mock<ILogger<IExpenseService>>();
        var userServiceMock = new Mock<IUserService>();

        
        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));

        var service = new ExpenseService(db, loggerMock.Object, userServiceMock.Object);
        var newUser = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@outlook.nl",
            PasswordHash = "Marvel01@"
        };
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashedPassword = hasher.HashPassword(newUser,newUser.PasswordHash);
        newUser.PasswordHash = hashedPassword;
        
        
        await db.Users.AddAsync(newUser);
        await db.SaveChangesAsync();
        
        var user = await db.Users.FirstAsync(user => user.Email == "johndoe@outlook.nl");
        

        Result<ResponseTransactionDTo>[] transactionArrayResult = new Result<ResponseTransactionDTo>[3];

        RequestTransactionDto[] transactions = new[] {

        new RequestTransactionDto
        {
            BucketId = 1,
            Description = "Salary payment",
            Amount = 1000,
            CreatedAt = new LocalDate(2025, 1, 20),
        },

        new RequestTransactionDto
        {
            BucketId = 2,
            Description = "Groceries at AH",
            Amount = 120,
            CreatedAt = new LocalDate(2025, 1, 12),
        },
        new RequestTransactionDto
        {
            BucketId = 3,
            Description = "Shopping - Clothing",
            Amount = 80,
            CreatedAt = new LocalDate(2025, 1, 29),
        }



    };

        for (var i = 0; i < transactionArrayResult.Length; i++)
        {
            var transactionIsCreated = await service.CreateTransaction(user.Id,transactions[i] );

            if (transactionIsCreated.IsSuccess)
            {
                transactionArrayResult[i] = transactionIsCreated;
            }
        }
        
        
        await db.SaveChangesAsync();

        // Assert
        Assert.Equal(3, transactionArrayResult.Count()); // Service method returns 3x true
        Assert.Equal(3, db.Transactions.Count()); // there are 3 transactions
        Assert.Equal(800, db.Buckets.First(bucket => bucket.Name == Buckets.Salary).Total);
        Assert.Equal(120, db.Buckets.First(bucket => bucket.Name == Buckets.Groceries).Total);
        Assert.Equal(80, db.Buckets.First(bucket => bucket.Name == Buckets.Shopping).Total);
    }

    [Fact]
    public async Task GetTransactions_ReturnsAllTransactions()
    {
        // Arrange
        await using  var db = _fixture.CreateContext();

        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var user = await db.Users.FirstAsync(user => user.Email == "arief@outlook.nl");




        // Act
        // Bucket  id=1 is Salary and id=2 is Groceries and id=3 is Shopping
        var firstFiveTransactions = new[]
        {
            new Transaction
            {
                Id=1,
                BucketId = 1,
                Description = "Monthly Salary",
                ApplicationUserId = user.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 1, 5)
            },
            new Transaction
            {
                Id=2,
                BucketId = 2,
                Description = "Groceries at the AH",
                ApplicationUserId = user.Id,

                Amount = 120,
                CreatedAt = new LocalDate(2025, 1, 12)
            },
            new Transaction
            {
                Id=3,
                BucketId = 3,
                Description = "Shopping - clothing",
                ApplicationUserId = user.Id,

                Amount = 80,
                CreatedAt = new LocalDate(2025, 1, 20)
            },
            new Transaction
            {
                Id=4,
                BucketId = 2,
                Description = "Weekly groceries",
                ApplicationUserId = user.Id,
                Amount = 95,
                CreatedAt = new LocalDate(2025, 2, 14)
            },
            new Transaction
            {
                Id=5,
                BucketId = 3,
                Description = "Shopping - online order",
                ApplicationUserId = user.Id,
                Amount = 150,
                CreatedAt = new LocalDate(2025, 3, 2)
            } };
        
        // Assert
        Assert.Equal(33, db.Transactions.Count());
        Assert.Equal(firstFiveTransactions, db.Transactions.OrderBy(transaction => transaction.Id).Take(5));
        // asserts op de waarde niet een heel object!
    }


    [Fact]
    public async Task TestDeletingTransactionById_Correct_Totals()
    {
        // Arrange

        await using var db = _fixture.CreateContext();
        var loggerMock = new Mock<ILogger<IExpenseService>>();
        
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);

        // Act
        // 3-> 80
        // 4 -> 95
        // 8 -> 110
        var T_3_isDeleted = await new ExpenseService(db,loggerMock.Object, userServiceMock.Object).DeleteTransaction(3);
        var T_4_isDeleted = await new ExpenseService(db, loggerMock.Object, userServiceMock.Object).DeleteTransaction(5);
        var T_8_isDeleted = await new ExpenseService(db, loggerMock.Object, userServiceMock.Object).DeleteTransaction(8);
        
        var currentShoppingTotal =  db.Buckets.First(bucket => bucket.Name == Buckets.Shopping).Total;
        var currentSalaryTotal = db.Buckets.First(bucket => bucket.Name == Buckets.Salary).Total;

        // Assert
        Assert.True(T_3_isDeleted.Value);
        Assert.True(T_4_isDeleted.Value);
        Assert.True(T_8_isDeleted.Value);
        Assert.Equal(815, currentShoppingTotal);
        Assert.Equal(6020, currentSalaryTotal);
    }

    [Fact]
    public async Task TestDeleteAllTransactions_Zero_As_Totals()
    {
        await using var db = _fixture.CreateContext();
        var loggerMock = new Mock<ILogger<IExpenseService>>();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        
        var expenseService = new ExpenseService(db, loggerMock.Object, userServiceMock.Object);
        var bucketService = new BucketService(db);

       

        foreach (var transaction in db.Transactions)
        {
            await expenseService.DeleteTransaction(transaction.Id);
        }

        var buckets = await bucketService.GetBuckets();
        Assert.NotNull(buckets.Value);
        
        var bucketsTotals = buckets.Value?.Count;
        Assert.Equal(3, bucketsTotals);
    }

    [Fact]
    public async Task TestInvalidCreationDateValidationAttribute()
    {
     // ARRANGE
     await using var db = _fixture.CreateContext();
     var userServiceMock = new Mock<IUserService>();

     userServiceMock
         .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
             dto.Email == "arief@outlook.nl" &&
             dto.FirstName == "John" &&
             dto.LastName == "Doe")))
         .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
         {
             Id = Guid.NewGuid().ToString(),
             Email = "arief@outlook.nl",
             FirstName = "John",
             LastName = "Doe"
         }));


     var seeder = new DbIntializer();
     await seeder.SeedAsync(db);
     
     var user = await db.Users.FirstAsync(user => user.Email == "arief@outlook.nl");
     
     Transaction[] newTransactions =
     {
         new Transaction{BucketId=1,
                        ApplicationUserId= user.Id,
                        Description="TestSalaris",
                        Amount=100,
                        CreatedAt= new LocalDate(2030,1,20),
                         },
         
         new Transaction{BucketId=3,
                        ApplicationUserId= user.Id,
                        Description="Shopping - clothing",
                        Amount=80,
                        CreatedAt= new LocalDate(2025,1,20),
                        },

     };
     // ACT
     db.Transactions.AddRange(newTransactions);
     await db.SaveChangesAsync();

     var attr = new CreatedAtValidation();
     var context = new ValidationContext(new { });
     var result = attr.GetValidationResult(newTransactions[0].CreatedAt, context);
     // ASSERT
     Assert.NotNull(result);
     Assert.NotEqual(ValidationResult.Success, result);
     Assert.Equal("The created date year must not be later than this year.", result.ErrorMessage);

    }

    [Fact]
    public async Task TestCorrectBucketSummaryJanuary2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        
        
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 1;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(month, year);

        Assert.NotNull(summary.Value);
        var value = summary.Value;
        
        var summaryTotalIncome = summary.Value?.TotalIncome;
        var summaryTotalExpenses = summary.Value?.TotalExpenses;
        // Assert
        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(385, summaryTotalExpenses);
        Assert.Equal(3,value.Buckets.Count);
        Assert.Contains(Buckets.Salary, value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,value.Month);
        Assert.Equal(year,value.Year);
        
    }
    [Fact]
    public async Task TestCorrectBucketSummaryMarch2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 3;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(month, year);
        
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;

        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(398, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    [Fact]
    public async Task TestCorrectBucketSummaryAugust2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        var bucketService = new BucketService(db);
        await seeder.SeedAsync(db);
        
        
        // Act
        const int month = 8;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(month, year);
        
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(108, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    [Fact]
    public async Task TestInCorrectBucketSummaryAugust2025()
    {
        // Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));


        var seeder = new DbIntializer();
        var bucketService = new BucketService(db);
        await seeder.SeedAsync(db);
        
        
        // Act
        const int month = 1;
        const int year = 2026;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = await bucketService.GetSummary(month, year);
        //Assert
        Assert.NotNull(summary.Value);
        
        var summaryTotalIncome = summary.Value.TotalIncome;
        var summaryTotalExpenses = summary.Value.TotalExpenses;
        

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(0, summaryTotalExpenses);
        Assert.Equal(3,summary.Value.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Value.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Value.Month);
        Assert.Equal(year,summary.Value.Year);
        
    }
    
    [Fact]
    public async Task TestTransactionUpdateById()
    {
        //Arrange
        await using var db = _fixture.CreateContext();
        var userServiceMock = new Mock<IUserService>();

        userServiceMock
            .Setup(x => x.RegisterAsync(It.Is<RegisterUserDto>(dto =>
                dto.Email == "arief@outlook.nl" &&
                dto.FirstName == "John" &&
                dto.LastName == "Doe")))
            .ReturnsAsync(Result<RegisteredUserDto>.Success(new RegisteredUserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = "arief@outlook.nl",
                FirstName = "John",
                LastName = "Doe"
            }));
        
        var seeder = new DbIntializer();
        await seeder.SeedAsync(db);
        
        var user = await db.Users.FirstAsync(user => user.Email == "arief@outlook.nl");

        var logger = new Mock<ILogger<IExpenseService>>();

        var expenseService = new ExpenseService(db, logger.Object, userServiceMock.Object);

        //Act
        await expenseService.UpdateTransaction(
            new Transaction
            {
                Id= 1,
                BucketId = 1,
                Description = "Weekly Salary",
                ApplicationUserId = user.Id,
                Amount = 100,
                CreatedAt = new LocalDate(2025, 1, 10)
            });
            
        var transaction = db.Transactions.First(transaction => transaction.Id == 1);

        // Assert
        Assert.Equal("Weekly Salary",transaction.Description );
        Assert.Equal(100,transaction.Amount );
        Assert.Equal(new LocalDate(2025,1, 10),transaction.CreatedAt );
        
       
    }

}


