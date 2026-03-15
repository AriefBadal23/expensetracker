using System.ComponentModel.DataAnnotations;
using expensetrackerapi.DTO;
using expensetrackerapi.Validation;

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

    public ExpenseTrackerTests(TestDbFixture fixture) => _fixture = fixture;

    [Fact]
    public void TestCreateTransaction()
    {
        // Arrange
        using var db = _fixture.CreateContext();

        // Act

        var buckets = new[]
        {
                new Bucket{Id=1, Name=Buckets.Salary,Icon="💰"},
                new Bucket{Id=2, Name=Buckets.Groceries,Icon="🏪"},
                new Bucket{Id=3, Name=Buckets.Shopping,Icon="🛒"}
        };

        db.Buckets.AddRange(buckets);
        db.SaveChanges();

        var service = new ExpenseService(db);

        ResponseTransactionDTo[] transactionArrayResult = new ResponseTransactionDTo[3];

        RequestTransactionDto[] transactions = new[] {
            
        new RequestTransactionDto
        {
            BucketId = 1,
            Description = "Salary payment",
            Amount = 1000,
            Created_at = new LocalDate(2025, 1, 20),
        },

        new RequestTransactionDto
        {
            BucketId = 2,
            Description = "Groceries at AH",
            Amount = 120,
            Created_at = new LocalDate(2025, 1, 12),
        },
        new RequestTransactionDto
        {
            BucketId = 3,
            Description = "Shopping - Clothing",
            Amount = 80,
            Created_at = new LocalDate(2025, 1, 29),
        }



    };

        for (var i = 0; i < transactionArrayResult.Length; i++)
        {
            var transactionIsCreated = service.CreateTransaction(transactions[i]);

            if (transactionIsCreated is not null)
            {
                transactionArrayResult[i] = transactionIsCreated;
            }
        }
        db.SaveChanges();

        // Assert
        Assert.Equal(3, transactionArrayResult.Count()); // Service method returns 3x true
        Assert.Equal(3, db.Transactions.Count()); // there are 3 transactions
        Assert.Equal(800, db.Buckets.First(bucket => bucket.Name == Buckets.Salary).Total);
        Assert.Equal(120, db.Buckets.First(bucket => bucket.Name == Buckets.Groceries).Total);
        Assert.Equal(80, db.Buckets.First(bucket => bucket.Name == Buckets.Shopping).Total);
    }

    [Fact]
    public void GetTransactions_ReturnsAllTransactions()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        DbIntializer.Initialize(db);



        // Act
        // Bucket  id=1 is Salary and id=2 is Groceries and id=3 is Shopping
        var firstFiveTransactions = new[]
        {
            new Transaction
            {
                Id=1,
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 1, 5)
            },
            new Transaction
            {
                Id=2,
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 120,
                Created_at = new LocalDate(2025, 1, 12)
            },
            new Transaction
            {
                Id=3,
                BucketId = 3,
                Description = "Shopping - clothing",
                UserId = 1,
                Amount = 80,
                Created_at = new LocalDate(2025, 1, 20)
            },
            new Transaction
            {
                Id=4,
                BucketId = 2,
                Description = "Weekly groceries",
                UserId = 1,
                Amount = 95,
                Created_at = new LocalDate(2025, 2, 14)
            },
            new Transaction
            {
                Id=5,
                BucketId = 3,
                Description = "Shopping - online order",
                UserId = 1,
                Amount = 150,
                Created_at = new LocalDate(2025, 3, 2)
            } };
        
        // Assert
        Assert.Equal(33, db.Transactions.Count());
        Assert.Equal(firstFiveTransactions, db.Transactions.OrderBy(transaction => transaction.Id).Take(5));
        // asserts op waarde niet hele object!
    }


    [Fact]
    public void TestDeletingTransactionById_Correct_Totals()
    {
        // Arrange

        using var db = _fixture.CreateContext();

        DbIntializer.Initialize(db);

        // Act
        // 3-> 80
        // 4 -> 95
        // 8 -> 110
        var T_3_isDeleted = new ExpenseService(db).DeleteTransaction(3);
        var T_4_isDeleted = new ExpenseService(db).DeleteTransaction(5);
        var T_8_isDeleted = new ExpenseService(db).DeleteTransaction(8);
        
        var currentShoppingTotal =  db.Buckets.First(_ => _.Name == Buckets.Shopping).Total;
        var currentSalaryTotal = db.Buckets.First(_ => _.Name == Buckets.Salary).Total;

        // Assert
        Assert.True(T_3_isDeleted);
        Assert.True(T_4_isDeleted);
        Assert.True(T_8_isDeleted);
        Assert.Equal(815, currentShoppingTotal);
        Assert.Equal(6020, currentSalaryTotal);
    }

    [Fact]
    public void TestDeleteAllTransactions_Zero_As_Totals()
    {
        using var db = _fixture.CreateContext();
        var expenseService = new ExpenseService(db);
        var bucketService = new BucketService(db);

        DbIntializer.Initialize(db);

        foreach (var transaction in db.Transactions)
        {
            expenseService.DeleteTransaction(transaction.Id);
        }

        int bucketsTotals = bucketService.GetBuckets().Count(_ => _.Total == 0);
        
        Assert.Equal(3, bucketsTotals);
    }

    [Fact]
    public void TestInvalidCreationDateValidationAttribute()
    {
     // ARRANGE
     using var db = _fixture.CreateContext();
     DbIntializer.Initialize(db);
     
     Transaction[] newTransactions =
     {
         new Transaction{BucketId=1,
                        UserId=1,
                        Description="TestSalaris",
                        Amount=100,
                        Created_at= new NodaTime.LocalDate(2030,1,20),
                         },
         
         new Transaction{BucketId=3,
                        UserId=1,
                        Description="Shopping - clothing",
                        Amount=80,
                        Created_at= new NodaTime.LocalDate(2025,1,20),
                        },

     };
     // ACT
     db.Transactions.AddRange(newTransactions);
     db.SaveChanges();

     var attr = new CreatedAtValidation();
     var context = new ValidationContext(new { });
     var result = attr.GetValidationResult(newTransactions[0].Created_at, context);
     // ASSERT
     Assert.NotEqual(ValidationResult.Success, result);
     Assert.Equal("The created date year must not be later than this year.", result.ErrorMessage);

    }

    [Fact]
    public void TestCorrectBucketSummaryJanuary2025()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        DbIntializer.Initialize(db);
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 1;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = bucketService.GetSummary(month, year);
        var summaryTotalIncome = summary.TotalIncome;
        var summaryTotalExpenses = summary.TotalExpenses;

        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(385, summaryTotalExpenses);
        Assert.Equal(3,summary.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Month);
        Assert.Equal(year,summary.Year);
        
    }
    [Fact]
    public void TestCorrectBucketSummaryMarch2025()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        DbIntializer.Initialize(db);
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 3;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = bucketService.GetSummary(month, year);
        var summaryTotalIncome = summary.TotalIncome;
        var summaryTotalExpenses = summary.TotalExpenses;

        Assert.Equal(1000, summaryTotalIncome);
        Assert.Equal(398, summaryTotalExpenses);
        Assert.Equal(3,summary.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Month);
        Assert.Equal(year,summary.Year);
        
    }
    [Fact]
    public void TestCorrectBucketSummaryAugust2025()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        DbIntializer.Initialize(db);
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 8;
        const int year = 2025;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = bucketService.GetSummary(month, year);
        var summaryTotalIncome = summary.TotalIncome;
        var summaryTotalExpenses = summary.TotalExpenses;

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(108, summaryTotalExpenses);
        Assert.Equal(3,summary.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Month);
        Assert.Equal(year,summary.Year);
        
    }
    [Fact]
    public void TestInCorrectBucketSummaryAugust2025()
    {
        // Arrange
        using var db = _fixture.CreateContext();
        DbIntializer.Initialize(db);
        var bucketService = new BucketService(db);
        
        
        // Act
        const int month = 1;
        const int year = 2026;
        
        // Uses the db to retrieve summary of transactions of the given month-year
        var summary = bucketService.GetSummary(month, year);
        var summaryTotalIncome = summary.TotalIncome;
        var summaryTotalExpenses = summary.TotalExpenses;
        

        Assert.Equal(0, summaryTotalIncome);
        Assert.Equal(0, summaryTotalExpenses);
        Assert.Equal(3,summary.Buckets.Count);
        Assert.Contains(Buckets.Salary, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Groceries, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Contains(Buckets.Shopping, summary.Buckets.Select(bucket => bucket.BucketName));
        Assert.Equal(month,summary.Month);
        Assert.Equal(year,summary.Year);
        
    }
    // UpdateTransaction    

}
