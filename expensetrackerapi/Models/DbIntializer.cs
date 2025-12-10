
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
            new User{Role=Role.User, Username="Arief"}
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

        // Make sure the Total is up-to-date of the buckets.
        var FirstupdateTotal = context.Buckets.First(x => x.Id == transactions[0].BucketId);
        FirstupdateTotal.Total = transactions[0].Amount;

        var SecondupdateTotal = context.Buckets.First(x => x.Id == transactions[1].BucketId);
        SecondupdateTotal.Total = transactions[1].Amount;

        context.Transactions.AddRange(transactions);
        context.SaveChanges();




    }
}
