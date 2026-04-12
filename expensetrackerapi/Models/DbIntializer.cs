using expensetrackerapi.Contracts;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace expensetrackerapi.Models;

public class DbIntializer:IDbInitializer
{
    private readonly IUserService _userService;
    
    public DbIntializer(IUserService userService)
    {
        _userService = userService;

    }
    
    public async Task SeedAsync(ExpenseTrackerContext context)
    {
        if (context.Buckets.Any())
        {
            return;
        }
        var buckets = new Bucket[]
        {
            new() {Name=Buckets.Salary,Icon="💰", Type = BucketTypes.Income},
            new() {Name=Buckets.Groceries,Icon="🏪", Type = BucketTypes.Expense},
            new() {Name=Buckets.Shopping,Icon="🛒", Type = BucketTypes.Expense}
        };

        await context.Buckets.AddRangeAsync(buckets);
        await context.SaveChangesAsync();


        var user = await _userService.RegisterAsync(new RegisterUserDto
        {
            Email = "arief@outlook.nl",
            FirstName = "Arief",
            LastName = "Badal",
            Password = "Marvel01@"
        });

      

        var transactions = new Transaction[]
        {
            new()
            {
                ApplicationUserId = user.Value!.Id,
                BucketId = 1,
                Description = "Monthly Salary",
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 1, 5)
            },
            new()
            {
                ApplicationUserId = user.Value!.Id,
                BucketId = 2,
                Description = "Groceries at the AH",
                Amount = 120,

                CreatedAt = new LocalDate(2025, 1, 12)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - clothing",
                ApplicationUserId = user.Value!.Id,
                Amount = 80,

                CreatedAt = new LocalDate(2025, 1, 20)
            },
            new()
            {
                BucketId = 2,
                Description = "Weekly groceries",
                ApplicationUserId = user.Value!.Id,
                Amount = 95,

                CreatedAt = new LocalDate(2025, 2, 14)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - online order",
               ApplicationUserId = user.Value!.Id,
                Amount = 150,

                CreatedAt = new LocalDate(2025, 3, 2)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,

                CreatedAt = new LocalDate(2025, 3, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries at the AH",
               ApplicationUserId = user.Value!.Id,
                Amount = 110,

                CreatedAt = new LocalDate(2025, 4, 10)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - electronics",
               ApplicationUserId = user.Value!.Id,
                Amount = 60,

                CreatedAt = new LocalDate(2025, 4, 18)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,

                CreatedAt = new LocalDate(2025, 5, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 105,
                CreatedAt = new LocalDate(2025, 1, 28)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - shoes",
               ApplicationUserId = user.Value!.Id,
                Amount = 140,
                CreatedAt = new LocalDate(2025, 2, 22)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 98,
                CreatedAt = new LocalDate(2025, 3, 18)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - gadgets",
               ApplicationUserId = user.Value!.Id,
                Amount = 75,
                CreatedAt = new LocalDate(2025, 4, 25)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 115,
                CreatedAt = new LocalDate(2025, 5, 19)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 6, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 102,
                CreatedAt = new LocalDate(2025, 6, 21)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - summer sale",
               ApplicationUserId = user.Value!.Id,
                Amount = 160,
                CreatedAt = new LocalDate(2025, 7, 9)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
                ApplicationUserId = user.Value!.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 7, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
                ApplicationUserId = user.Value!.Id,
                Amount = 108,
                CreatedAt = new LocalDate(2025, 8, 14)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - backpack",
                ApplicationUserId = user.Value!.Id,
                Amount = 90,
                CreatedAt = new LocalDate(2025, 9, 3)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 9, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 112,
                CreatedAt = new LocalDate(2025, 10, 11)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - jacket",
               ApplicationUserId = user.Value!.Id,
                Amount = 180,
                CreatedAt = new LocalDate(2025, 11, 6)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 2, 5)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = user.Value!.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 12, 5)
            },
            new()
            {
                BucketId = 2,
                Description = "Extra groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 45,
                CreatedAt = new LocalDate(2025, 1, 8)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - small items",
               ApplicationUserId = user.Value!.Id,
                Amount = 35,
                CreatedAt = new LocalDate(2025, 1, 18)
            },
            new()
            {
                BucketId = 2,
                Description = "Extra groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 55,
                CreatedAt = new LocalDate(2025, 3, 10)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - accessories",
               ApplicationUserId = user.Value!.Id,
                Amount = 65,
                CreatedAt = new LocalDate(2025, 3, 22)
            },
            new()
            {
                BucketId = 2,
                Description = "Groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 48,
                CreatedAt = new LocalDate(2025, 7, 12)
            },
            new()
            {
                BucketId = 3,
                Description = "Shopping - sale item",
               ApplicationUserId = user.Value!.Id,
                Amount = 40,
                CreatedAt = new LocalDate(2025, 7, 20)
            },
            new()
            {
                BucketId = 2,
                Description = "Late groceries",
               ApplicationUserId = user.Value!.Id,
                Amount = 52,
                CreatedAt = new LocalDate(2025, 7, 27)
            },
            new()
            {
                BucketId = 3,
                Description = "Impulse buy",
                ApplicationUserId = user.Value!.Id,
                Amount = 30,
                CreatedAt = new LocalDate(2025, 3, 28)
            }




        };


        // Make sure the Total is up-to-date of the buckets.

        var newbuckets = context.Buckets.ToDictionary(b => b.Id);
        var salaryBucket = newbuckets.Values.First(b => b.Name == Buckets.Salary);
        foreach (var bucket in newbuckets.Values)
        {
            bucket.Total = 0;
        }

        foreach (var t in transactions)
        {
            var bucket = context.Buckets.First(x => x.Id == t.BucketId);

            if (bucket.Name == Buckets.Salary && bucket.Type == BucketTypes.Income)
            {
                // Update Salary total if it's an income.
                bucket.Total += t.Amount;
            }
            else
            {
                // Update the bucket & Salary Total
                bucket.Total += t.Amount;
                salaryBucket.Total -= t.Amount;
            }


        }
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}
