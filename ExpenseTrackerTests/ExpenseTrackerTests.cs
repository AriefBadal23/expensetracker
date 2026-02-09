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

        bool[] transactionArrayResult = new bool[3];

        CreateTransactionDto[] dto = new[] {
        new CreateTransactionDto
        {
            BucketId = 1,
            Description = "Salary payment",
            Amount = 1000,
            CreatedAt = new LocalDate(2025, 1, 20),
            IsIncome = true
        },

        new CreateTransactionDto
        {
            BucketId = 2,
            Description = "Groceries at AH",
            Amount = 120,
            CreatedAt = new LocalDate(2025, 1, 12),
            IsIncome = false
        },
        new CreateTransactionDto
        {
            BucketId = 3,
            Description = "Shopping - Clothing",
            Amount = 80,
            CreatedAt = new LocalDate(2025, 1, 29),
            IsIncome = false
        }



    };

        for (var i = 0; i < transactionArrayResult.Length; i++)
        {
            var TransactionIsCreated = service.CreateTransaction(dto[i]);

            if (TransactionIsCreated)
            {
                transactionArrayResult[i] = TransactionIsCreated;
            }
        }
        db.SaveChanges();

        // Assert
        Assert.Equal(3, transactionArrayResult.Count(_ => _ == true)); // Service method returns 3x true
        Assert.Equal(3, db.Transactions.Count()); // there are 3 transactions
        Assert.Equal(800, db.Buckets.Where(_ => _.Name == Buckets.Salary).First().Total);
        Assert.Equal(120, db.Buckets.Where(_ => _.Name == Buckets.Groceries).First().Total);
        Assert.Equal(80, db.Buckets.Where(_ => _.Name == Buckets.Shopping).First().Total);
    }

    [Fact]
    public void GetTransactions_ReturnsAllTransactions()
    {
        // Arrange
        using var _db = _fixture.CreateContext();
        DbIntializer.Initialize(_db);



        // Act
        var transactions = new ExpenseService(_db).GetTransactions(1, 2025, 3);
        // Bucket  id=1 is Salary and id=2 is Groceries and id=3 is Shopping

        // Assert
        var salaryTransAction = _db.Transactions.Where(x => x.IsIncome == true).First();
        var firstFiveTransactions = new Transaction[]
        {
            new Transaction
            {
                Id=1,
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                IsIncome = true,
                Created_at = new LocalDate(2025, 1, 5)
            },
            new Transaction
            {
                Id=2,
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 120,
                IsIncome = false,
                Created_at = new LocalDate(2025, 1, 12)
            },
            new Transaction
            {
                Id=3,
                BucketId = 3,
                Description = "Shopping - clothing",
                UserId = 1,
                Amount = 80,
                IsIncome = false,
                Created_at = new LocalDate(2025, 1, 20)
            },
            new Transaction
            {
                Id=4,
                BucketId = 2,
                Description = "Weekly groceries",
                UserId = 1,
                Amount = 95,
                IsIncome = false,
                Created_at = new LocalDate(2025, 2, 14)
            },
            new Transaction
            {
                Id=5,
                BucketId = 3,
                Description = "Shopping - online order",
                UserId = 1,
                Amount = 150,
                IsIncome = false,
                Created_at = new LocalDate(2025, 3, 2)
            } };

        Assert.Equal(33, _db.Transactions.Count());
        Assert.Equal(1, salaryTransAction.Id);
        Assert.Equal("Monthly Salary", salaryTransAction.Description);
        Assert.Equal(firstFiveTransactions, _db.Transactions.OrderBy(_ => _.Id).Take(5));

        // asserts op waarde niet hele object!
    }


    [Fact]
    public void TestDeletingTransactionById_Correct_Totals()
    {
        // Arrange

        using var _db = _fixture.CreateContext();


        Transaction[] newTransactions =
        {
            new Transaction{Id=3,BucketId=1,UserId=1,Description="TestSalaris",Amount=100,Created_at=new NodaTime.LocalDate(2025,1,20), IsIncome=true },
            new Transaction{Id=4,BucketId=3,UserId=1,Description="Shopping - clothing",Amount=80,Created_at=new NodaTime.LocalDate(2025,1,20), IsIncome=false },

        };

        DbIntializer.Initialize(_db);

        // Act

        var shoppingTotal = _db.Buckets.First(_ => _.Name == Buckets.Shopping).Total;
        var salaryTotal = _db.Buckets.First(_ => _.Name == Buckets.Salary).Total;

        var deletedTransaction = _db.Transactions.Where(x => x.Id == 3).First();

        var isDeleted = new ExpenseService(_db).DeleteTransaction(3);
        var updatedShoppingTotal = _db.Buckets.First(_ => _.Name == Buckets.Shopping).Total;
        var updatedSalaryTotal = _db.Buckets.First(_ => _.Name == Buckets.Salary).Total;

        // Assert
        Assert.True(isDeleted);
        Assert.Equal(1025, updatedShoppingTotal);
        Assert.Equal(5810, updatedSalaryTotal);
    }

}
