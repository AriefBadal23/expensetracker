
using expensetrackerapi.Contracts;
using expensetrackerapi.helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace expensetrackerapi.Models;

public class DbIntializer:IDbInitializer
{
    // 💡 First I used UserService in this class (constructor DI) but it made it hard to use it in the unittests.
    // Now I make the ApplicationUser object, hash the password and store the user in the DB without any userService methods.
    public async Task SeedAsync(ExpenseTrackerContext context)
    {
        
        if (context.Buckets.Any())
        {
            return;
        }
        Console.WriteLine("Database seeding started");
        
        var hasher = new PasswordHasher<ApplicationUser>();
        
        var user = new ApplicationUser()
        {
            Email = "arief@outlook.nl",
            FirstName = "John",
            LastName = "Doe",
            UserName = "arief@outlook.nl",
            NormalizedUserName = "ARIEF@OUTLOOK.NL",
            NormalizedEmail = "ARIEF@OUTLOOK.NL",
            EmailConfirmed = false,
            SecurityStamp = Guid.NewGuid().ToString("D")
            
        };
        
        user.PasswordHash = hasher.HashPassword(user, "Marvel01@"); ;

        await context.Users.AddAsync(user);
        
        Console.WriteLine($"{await context.SaveChangesAsync()} User has been added to the database.");
        
        var buckets = new Bucket[]
        {
            new() {Name=Buckets.Salary,Icon="💰", Type = BucketTypes.Income},
            new() {Name=Buckets.Groceries,Icon="🏪", Type = BucketTypes.Expense},
            new() {Name=Buckets.Shopping,Icon="🛒", Type = BucketTypes.Expense}
        };

        await context.Buckets.AddRangeAsync(buckets);
        await context.SaveChangesAsync();
        
        var userBuckets = buckets.Select(bucket => new UserBuckets { ApplicationUserId = user.Id, BucketId = bucket.Id }).ToList();
        await context.UserBuckets.AddRangeAsync(userBuckets);
        Console.WriteLine($"{await context.SaveChangesAsync()} Buckets has been added to the database.");
        
        // Seeding of M:M table (Join Entity Type)
        var newUser = await context.Users.FirstAsync(u => u.Email == "arief@outlook.nl");
        
        var transactions = new Transaction[]
        {
            new()
            {
                ApplicationUserId = newUser.Id,
                BucketId = 1,
                Description = "Monthly Salary",
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 1, 5)
            },
            new()
            {
                ApplicationUserId = newUser.Id,
                BucketId = 3,
                Description = "Groceries at the AH",
                Amount = 120,

                CreatedAt = new LocalDate(2025, 1, 12)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - clothing",
                ApplicationUserId = newUser.Id,
                Amount = 80,

                CreatedAt = new LocalDate(2025, 1, 20)
            },
            new()
            {
                BucketId = 3,
                Description = "Weekly groceries",
                ApplicationUserId = newUser.Id,
                Amount = 95,

                CreatedAt = new LocalDate(2025, 2, 14)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - online order",
               ApplicationUserId = newUser.Id,
                Amount = 150,

                CreatedAt = new LocalDate(2025, 3, 2)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,

                CreatedAt = new LocalDate(2025, 3, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries at the AH",
               ApplicationUserId = newUser.Id,
                Amount = 110,

                CreatedAt = new LocalDate(2025, 4, 10)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - electronics",
               ApplicationUserId = newUser.Id,
                Amount = 60,

                CreatedAt = new LocalDate(2025, 4, 18)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,

                CreatedAt = new LocalDate(2025, 5, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 105,
                CreatedAt = new LocalDate(2025, 1, 28)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - shoes",
               ApplicationUserId = newUser.Id,
                Amount = 140,
                CreatedAt = new LocalDate(2025, 2, 22)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 98,
                CreatedAt = new LocalDate(2025, 3, 18)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - gadgets",
               ApplicationUserId = newUser.Id,
                Amount = 75,
                CreatedAt = new LocalDate(2025, 4, 25)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 115,
                CreatedAt = new LocalDate(2025, 5, 19)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 6, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 102,
                CreatedAt = new LocalDate(2025, 6, 21)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - summer sale",
               ApplicationUserId = newUser.Id,
                Amount = 160,
                CreatedAt = new LocalDate(2025, 7, 9)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
                ApplicationUserId = newUser.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 7, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
                ApplicationUserId = newUser.Id,
                Amount = 108,
                CreatedAt = new LocalDate(2025, 8, 14)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - backpack",
                ApplicationUserId = newUser.Id,
                Amount = 90,
                CreatedAt = new LocalDate(2025, 9, 3)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 9, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 112,
                CreatedAt = new LocalDate(2025, 10, 11)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - jacket",
               ApplicationUserId = newUser.Id,
                Amount = 180,
                CreatedAt = new LocalDate(2025, 11, 6)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 2, 5)
            },
            new()
            {
                BucketId = 1,
                Description = "Monthly Salary",
               ApplicationUserId = newUser.Id,
                Amount = 1000,
                CreatedAt = new LocalDate(2025, 12, 5)
            },
            new()
            {
                BucketId = 3,
                Description = "Extra groceries",
               ApplicationUserId = newUser.Id,
                Amount = 45,
                CreatedAt = new LocalDate(2025, 1, 8)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - small items",
               ApplicationUserId = newUser.Id,
                Amount = 35,
                CreatedAt = new LocalDate(2025, 1, 18)
            },
            new()
            {
                BucketId = 3,
                Description = "Extra groceries",
               ApplicationUserId = newUser.Id,
                Amount = 55,
                CreatedAt = new LocalDate(2025, 3, 10)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - accessories",
               ApplicationUserId = newUser.Id,
                Amount = 65,
                CreatedAt = new LocalDate(2025, 3, 22)
            },
            new()
            {
                BucketId = 3,
                Description = "Groceries",
               ApplicationUserId = newUser.Id,
                Amount = 48,
                CreatedAt = new LocalDate(2025, 7, 12)
            },
            new()
            {
                BucketId = 2,
                Description = "Shopping - sale item",
               ApplicationUserId = newUser.Id,
                Amount = 40,
                CreatedAt = new LocalDate(2025, 7, 20)
            },
            new()
            {
                BucketId = 3,
                Description = "Late groceries",
               ApplicationUserId = newUser.Id,
                Amount = 52,
                CreatedAt = new LocalDate(2025, 7, 27)
            },
            new()
            {
                BucketId = 2,
                Description = "Impulse buy",
                ApplicationUserId = newUser.Id,
                Amount = 30,
                CreatedAt = new LocalDate(2025, 3, 28)
            }

        };

        // Make sure the Total is up-to-date of the buckets.

        var newbuckets = context.Buckets.ToDictionary(b => b.Id);
        var salaryBucket = await context.UserBuckets.FirstAsync(ub => ub.BucketId == 1 && ub.ApplicationUserId == user.Id);
        
        var totals = await new BucketQueries(context).GetTotals(user.Id);
        
        foreach (var bucket in newbuckets.Values)
        {
             totals[bucket.Id] = 0;
        }

        foreach (var t in transactions)
        {
            var userBucket =
                await context.UserBuckets.FirstAsync(ub => ub.ApplicationUserId == user.Id && ub.BucketId == t.BucketId);
            
            var bucket = context.Buckets.First(x => x.Id == t.BucketId);

            if (bucket.Name == Buckets.Salary && bucket.Type == BucketTypes.Income)
            {
                // Update Salary total if it's an income.
                userBucket.Total += t.Amount;
            }
            else
            {
                // Update the bucket & Salary Total
                salaryBucket.Total-= t.Amount;
                userBucket.Total += t.Amount;
                

            }


        }
        
        await context.Transactions.AddRangeAsync(transactions);
        
        
        Console.WriteLine($"{await context.SaveChangesAsync()} transactions are added.");
        Console.WriteLine("Database seeding completed");
    }
    
        
    }
