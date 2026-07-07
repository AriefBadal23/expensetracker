using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;
using Microsoft.EntityFrameworkCore;

namespace expensetrackerapi.Services;


public class BucketService : IBucketService
{
    private readonly ExpenseTrackerContext _db;
    private readonly ILogger<BucketService> _logger;

    public BucketService(ExpenseTrackerContext context, ILogger<BucketService> logger)
    {
        _db = context;
        _logger = logger;
    }


    public async Task<Result<List<UserBucketResponseDto>>> GetBucketsByUserId(string? userId)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userDoesExists)
        {
            _logger.LogWarning("Failed to retrieve buckets due invalid userId for userId: {UserId}", userId);
            return Result<List<UserBucketResponseDto>>.Failure();
        }

        var buckets = from bucket in _db.Buckets
                      join userbucket in _db.UserBuckets on bucket.Id equals userbucket.BucketId into Userbucketgroup

                      from userbucket in Userbucketgroup
                      where userbucket.ApplicationUserId == userId
                      select new UserBucketResponseDto
                      {
                          Bucket = bucket,
                          BucketTotal = userbucket.Total
                      };

        _logger.LogInformation("Successfully retrieved buckets by userId: {UserId}", userId);
        return Result<List<UserBucketResponseDto>>.Success(
            await buckets.ToListAsync());
    }

    public async Task<Result<UserBucketResponseDto>> CreateBucket(string? userId, BucketRequestDto bucket)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userDoesExists)
        {
            return Result<UserBucketResponseDto>.Failure();
        }


        Bucket newBucket = new Bucket
        {
            Icon = bucket.Icon,
            Name = bucket.Name,
            Type = bucket.Type
        };




        await _db.Buckets.AddAsync(newBucket);
        await _db.SaveChangesAsync();

        var bucketId = await _db.Buckets.FirstAsync(b => b.Name == bucket.Name);
        var userBucket = new UserBuckets { ApplicationUserId = userId, BucketId = bucketId.Id };

        await _db.UserBuckets.AddAsync(userBucket);
        await _db.SaveChangesAsync();

        // THe code before cause issues with showing the total of the newly created bucket.
        var userBucketTotal = userBucket.Total;

        return Result<UserBucketResponseDto>.Success(new UserBucketResponseDto
        {
            Bucket = newBucket,
            BucketTotal = userBucketTotal
        });

    }

    public async Task<Result<BucketSummaryResponseDto>> GetSummary(string userId, int month, int year)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);

        if (!userDoesExists)
        {
            _logger.LogWarning("Failed to retrieve transactions summary due invalid userId for userId: {UserId}", userId);
            return Result<BucketSummaryResponseDto>.Failure();
        }

        if (month == 0 || year == 0)
        {
            _logger.LogInformation("Successfully retrieved Bucket Transactions summary by userId for {UserId} without month and year", userId);
            return
                Result<BucketSummaryResponseDto>.Success(new BucketSummaryResponseDto
                {
                    Buckets = new List<BucketTransaction>()
                });
        }

        var query = await (from buck in _db.Buckets
                           join transaction in _db.Transactions on buck.Id equals transaction.BucketId
                               into bucketTransactions

                           let userTransactions = bucketTransactions
                               .Where(t => t.CreatedAt.Month == month && t.CreatedAt.Year == year && t.ApplicationUserId == userId)


                           let monthBucketTotal = userTransactions.Sum(x => x.Amount)

                           select
                                   new BucketTransaction(buck.Id, buck.Name, buck.Type, monthBucketTotal, userTransactions.ToArray())
            ).ToListAsync();


        // Make use of the query but change the return type so it matches the required output for the front-end.
        _logger.LogInformation("Successfully retrieved Bucket Transactions summary by userId for {UserId}.", userId);

        return Result<BucketSummaryResponseDto>.Success(
            new BucketSummaryResponseDto
            {
                Month = month,
                Year = year,
                Buckets = query,
                TotalExpenses = query.Where(x => x.BucketName != nameof(Buckets.Salary)).Sum(x => x.BucketExpenseTotal),
                TotalIncome = query.Where(x => x.BucketName == nameof(Buckets.Salary)).Sum(x => x.BucketExpenseTotal),
            });

    }

    public async Task<Result<bool>> DeleteBucket(string? userId, int bucketId)
    {
        var bucket = await _db.Buckets.FindAsync(bucketId);
        var userBucket = await _db.UserBuckets.FindAsync(userId,bucketId);
        var defaultBuckets = new string[] { nameof(Buckets.Groceries), nameof(Buckets.Salary), nameof(Buckets.Shopping) };

        // finds the bucket that will be deleted.
        var deletedBucket =
            await _db.UserBuckets.FirstOrDefaultAsync(ub =>
                ub.ApplicationUserId == userId && ub.BucketId == bucketId);
        
        
        if (deletedBucket is not null)
        {
            var totalDeletedBucket  =  deletedBucket.Total;
            var userSalaryBucket = await _db.UserBuckets.FirstAsync(ub => ub.ApplicationUserId == userId && ub.BucketId == 1);
            
            // delete bucket and user bucket row 
            if (bucket is not null
                && userBucket is not null
                && !defaultBuckets.Contains(bucket.Name)
                )
            {
                // Make sure the bucket total moves back to the salary bucket.
                 userSalaryBucket.Total += totalDeletedBucket;
                _db.Buckets.Remove(bucket);
                _db.UserBuckets.Update(userSalaryBucket);
                _db.UserBuckets.Remove(userBucket);
                await _db.SaveChangesAsync();
                return Result<bool>.Success(true);

            }
        }
        

        return Result<bool>.Failure();
    }

}
