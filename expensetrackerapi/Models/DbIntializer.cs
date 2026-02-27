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
                new Bucket{Name=Buckets.Salary,Icon="💰", Type = BucketTypes.Income},
                new Bucket{Name=Buckets.Groceries,Icon="🏪", Type = BucketTypes.Expense},
                new Bucket{Name=Buckets.Shopping,Icon="🛒", Type = BucketTypes.Expense}
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
                Created_at = new LocalDate(2025, 1, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 120,
                
                Created_at = new LocalDate(2025, 1, 12)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - clothing",
                UserId = 1,
                Amount = 80,
                
                Created_at = new LocalDate(2025, 1, 20)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Weekly groceries",
                UserId = 1,
                Amount = 95,
                
                Created_at = new LocalDate(2025, 2, 14)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - online order",
                UserId = 1,
                Amount = 150,
                
                Created_at = new LocalDate(2025, 3, 2)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
            
                Created_at = new LocalDate(2025, 3, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries at the AH",
                UserId = 1,
                Amount = 110,
                
                Created_at = new LocalDate(2025, 4, 10)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - electronics",
                UserId = 1,
                Amount = 60,
                
                Created_at = new LocalDate(2025, 4, 18)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
               
                Created_at = new LocalDate(2025, 5, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 105,
                Created_at = new LocalDate(2025, 1, 28)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - shoes",
                UserId = 1,
                Amount = 140,
                Created_at = new LocalDate(2025, 2, 22)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 98,
                Created_at = new LocalDate(2025, 3, 18)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - gadgets",
                UserId = 1,
                Amount = 75,
                Created_at = new LocalDate(2025, 4, 25)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 115,
                Created_at = new LocalDate(2025, 5, 19)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 6, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 102,
                Created_at = new LocalDate(2025, 6, 21)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - summer sale",
                UserId = 1,
                Amount = 160,
                Created_at = new LocalDate(2025, 7, 9)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 7, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 108,
                Created_at = new LocalDate(2025, 8, 14)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - backpack",
                UserId = 1,
                Amount = 90,
                Created_at = new LocalDate(2025, 9, 3)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 9, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 112,
                Created_at = new LocalDate(2025, 10, 11)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - jacket",
                UserId = 1,
                Amount = 180,
                Created_at = new LocalDate(2025, 11, 6)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 2, 5)
            },
            new Transaction
            {
                BucketId = 1,
                Description = "Monthly Salary",
                UserId = 1,
                Amount = 1000,
                Created_at = new LocalDate(2025, 12, 5)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Extra groceries",
                UserId = 1,
                Amount = 45,
                Created_at = new LocalDate(2025, 1, 8)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - small items",
                UserId = 1,
                Amount = 35,
                Created_at = new LocalDate(2025, 1, 18)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Extra groceries",
                UserId = 1,
                Amount = 55,
                Created_at = new LocalDate(2025, 3, 10)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - accessories",
                UserId = 1,
                Amount = 65,
                Created_at = new LocalDate(2025, 3, 22)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Groceries",
                UserId = 1,
                Amount = 48,
                Created_at = new LocalDate(2025, 7, 12)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Shopping - sale item",
                UserId = 1,
                Amount = 40,
                Created_at = new LocalDate(2025, 7, 20)
            },
            new Transaction
            {
                BucketId = 2,
                Description = "Late groceries",
                UserId = 1,
                Amount = 52,
                Created_at = new LocalDate(2025, 7, 27)
            },
            new Transaction
            {
                BucketId = 3,
                Description = "Impulse buy",
                UserId = 1,
                Amount = 30,
                Created_at = new LocalDate(2025, 3, 28)
            }




        };


        // Make sure the Total is up-to-date of the buckets.

        var Newbuckets = context.Buckets.ToDictionary(b => b.Id);
        var salaryBucket = Newbuckets.Values.First(b => b.Name == Buckets.Salary);
        foreach (var bucket in Newbuckets.Values)
        {
            bucket.Total = 0;
        }

        foreach (var t in transactions)
        {
            var bucket = context.Buckets.First(x => x.Id == t.BucketId);

            if (bucket.Name == Buckets.Salary && bucket.Type == BucketTypes.Income)
            {
                // Update Salary total if its an income.
                bucket.Total += t.Amount;
            }
            else
            {
                // Update the bucket & Salary Total
                bucket.Total += t.Amount;
                salaryBucket.Total -= t.Amount;
            }


        }
        context.Transactions.AddRange(transactions);
        context.SaveChanges();
    }
}
