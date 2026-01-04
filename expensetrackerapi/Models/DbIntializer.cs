
using NodaTime;

namespace expensetrackerapi.Models;

public static class DbIntializer
{
    public static void Initialize(ExpenseTrackerContext context)
    {
        if (context.Buckets.Any())
        {
            return;
        }

        var buckets = new Bucket[]
        {
                new Bucket{Name=Buckets.Salary,Icon="💰"},
                new Bucket{Name=Buckets.Groceries,Icon="🏪"},
                new Bucket{Name=Buckets.Shopping,Icon="🛒"}
        };

        context.Buckets.AddRange(buckets);
        context.SaveChanges();

        var users = new User[]
        {
            new User{Role=Role.User, Username="Arief", Password="Test1234"}
        };


        context.AddRange(users);
        context.SaveChanges();




        var transactions = new Transaction[]
        {
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                IsIncome = true,
                Created_at = new LocalDate(2025, 1, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 120,
                IsIncome = false,
                Created_at = new LocalDate(2025, 1, 12)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - clothing",
                UserId = 1,
                Amount = 80,
                IsIncome = false,
                Created_at = new LocalDate(2025, 1, 20)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                IsIncome = true,
                Created_at = new LocalDate(2025, 2, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Weekly groceries",
                UserId = 1,
                Amount = 95,
                IsIncome = false,
                Created_at = new LocalDate(2025, 2, 14)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - online order",
                UserId = 1,
                Amount = 150,
                IsIncome = false,
                Created_at = new LocalDate(2025, 3, 2)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                IsIncome = true,
                Created_at = new LocalDate(2025, 3, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 110,
                IsIncome = false,
                Created_at = new LocalDate(2025, 4, 10)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - electronics",
                UserId = 1,
                Amount = 60,
                IsIncome = false,
                Created_at = new LocalDate(2025, 4, 18)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                IsIncome = true,
                Created_at = new LocalDate(2025, 5, 5)
            }


        };


        // Make sure the Total is up-to-date of the buckets.
        var FirstupdateTotal = context.Buckets.First(x => x.Id == transactions[0].BucketId);
        FirstupdateTotal.Total = transactions[0].Amount;

        var SecondupdateTotal = context.Buckets.First(x => x.Id == transactions[1].BucketId);
        SecondupdateTotal.Total = transactions[1].Amount;

        context.Transactions.AddRange(transactions);
        context.SaveChanges();


        var Salary = context.Buckets.First(x => x.Name == Buckets.Salary);

        if (Salary.Total > 0)
        {
            Salary.Total = Salary.Total - SecondupdateTotal.Total;
            context.Update(Salary);
            context.SaveChanges();
        }





    }
}
