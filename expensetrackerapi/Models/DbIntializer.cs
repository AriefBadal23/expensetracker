
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
            new User{Role=Role.User, Username="Arief", Created_at= DateTime.UtcNow.Date}
        };


        context.AddRange(users);
        context.SaveChanges();




        var transactions = new Transaction[]
        {
            new Transaction
            {
                BucketId=2,
                Description="Groceries at the AH",
                UserId=1,
                Amount=100
            },
            new Transaction
            {
                BucketId=1,
                Description="New Jacket",
                UserId=1,
                Amount=350
            },
        };

        context.Transactions.AddRange(transactions);
        context.SaveChanges();




    }
}
